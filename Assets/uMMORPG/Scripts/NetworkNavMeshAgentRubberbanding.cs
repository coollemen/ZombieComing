// Rubberband navmesh movement. Client moves, sends move to server, server
// rejects it if necessary.
//
// There are a lot of things to consider:
// - it needs to work for click movement (agent.destination)
// - it needs to work for wasd movement (agent.velocity)
// - server needs to broadcast the move to other clients
// - other clients need to see the correct positions after joining the area
// - server needs to check/reject positions if needed
// - agent.warp (on server) needs to be either detected and forced to clients,
//   or server scripts need to call ForcePositionToClients or similar
// - players shouldn't move while DEAD, TRADING, etc.
//
// The great part about this solution is that the client can move freely, but
// the server can still intercept with:
//   * agent.Warp
//   * agent.destination
//   * agent.ResetPath
// => all those calls are detected here and forced to the client.
//
// Note: no LookAtY needed because we move everything via .destination
using UnityEngine;
using UnityEngine.AI;
using Mirror;

[RequireComponent(typeof(NavMeshAgent))]
[NetworkSettings(sendInterval=0.1f)]
public class NetworkNavMeshAgentRubberbanding : NetworkBehaviour
{
    public NavMeshAgent agent; // assign in Inspector (instead of GetComponent)
    public Entity entity;

    // remember last serialized values for dirty bit
    Vector3 lastServerPosition;
    Vector3 lastSentDestination;
    Vector3 lastSentPosition;
    Vector3 lastReceivedDestination;
    float lastSentTime;
    bool hadPath;

    // epsilon for float/vector3 comparison (needed because of imprecision
    // when sending over the network, etc.)
    const float epsilon = 0.1f;

    // validate a move (the 'rubber' part)
    bool ValidateMove(Vector3 position)
    {
        // there is virtually no way to cheat navmesh movement, since it will
        // never calcluate a path to a point that is not on the navmesh.
        // -> we only need to check if alive
        // -> maybe a distance check in case we get too far off from latency
        return entity.health > 0;
    }

    [Command]
    void CmdMovedClick(Vector3 destination, float stoppingDistance)
    {
        // rubberband (check if valid move)
        if (ValidateMove(destination))
        {
            // apply the move on the server
            agent.stoppingDistance = stoppingDistance;
            agent.destination = destination;
            lastReceivedDestination = destination;

            // set dirty to trigger a OnSerialize next time, so that other clients
            // know about the new position too
            SetDirtyBit(1);
        }
        else
        {
            // otherwise keep current position and set dirty so that OnSerialize
            // is trigger. it will warp eventually when getting too far away.
            SetDirtyBit(1);
        }
    }

    [Command]
    void CmdMovedWASD(Vector3 position)
    {
        // rubberband (check if valid move)
        if (ValidateMove(position))
        {
            // set position via .destination to get free interpolation
            agent.stoppingDistance = 0;
            agent.destination = position;
            lastReceivedDestination = position;

            // set dirty to trigger a OnSerialize next time, so that other clients
            // know about the new position too
            SetDirtyBit(1);
        }
        else
        {
            // otherwise keep current position and set dirty so that OnSerialize
            // is trigger. it will warp eventually when getting too far away.
            SetDirtyBit(1);
        }
    }

    bool HasPath()
    {
        return agent.hasPath || agent.pathPending; // might still be computed
    }

    void Update()
    {
        // detect move mode
        bool hasPath = HasPath();

        // server should detect teleports / react if we got too far off
        // do this BEFORE isLocalPlayer actions so that agent.ResetPath can be
        // detected properly? otherwise localplayer wasdmovement cmd may
        // overwrite it
        if (isServer)
        {
            // neither click or wasd movement, but position changed further than 'speed'?
            // then we must have teleported, no other way to move this fast.
            if (!hasPath && agent.velocity == Vector3.zero &&
                Vector3.Distance(transform.position, lastServerPosition) > agent.speed)
            {
                // set NetworkNavMeshAgent dirty so that onserialize is
                // triggered and the client receives the position change
                SetDirtyBit(1);
                //Debug.Log(name + " teleported!");
            }

            // different destination than the one that we received from the
            // client?
            // (epsilon comparison needed for float precision over the network)
            if (HasPath() && Vector3.Distance(agent.destination, lastReceivedDestination) > epsilon)
            {
                // set dirty so onserialize notifies others
                SetDirtyBit(1);
                //Debug.Log(name + " destination changed");

                // send target rpc to the local player so he doesn't ignore it
                // -> better than a 'bool forceReset' flag for OnSerialize
                //    because new Cmds might come in before OnSerialize was
                //    called, which could lead to race conditions
                TargetSetDestination(connectionToClient, agent.destination, agent.stoppingDistance);
            }

            // detect agent.Reset:
            // - had a path before but not anymore?
            // - and we never reached the planned destination even though path was canceled?
            // (epsilon comparison needed for float precision over the network)
            if (hadPath && !hasPath && Vector3.Distance(transform.position, lastReceivedDestination) > epsilon)
            {
                // set dirty so onserialize notifies others
                SetDirtyBit(1);
                //Debug.Log(name + " path was reset");

                // send target rpc to the local player so he doesn't ignore it
                // -> better than a 'bool forceReset' flag for OnSerialize
                //    because new Cmds might come in before OnSerialize was
                //    called, which could lead to race conditions
                TargetResetMovement(connectionToClient);
            }

            lastServerPosition = transform.position;
            hadPath = hasPath;
        }

        // local player can move freely
        if (isLocalPlayer)
        {
            // click movement and destination changed since last sync?
            // then send the latest DESTINATION
            if (hasPath && agent.destination != lastSentDestination)
            {
                // don't send all the time to not DDOS the server
                // note: if we check time BEFORE detecting move then moves are
                //       detected very late and cause additional latency and
                //       might cause move->idle->move effect if second move
                //       doesn't come in fast enough, etc.
                if (Time.time > lastSentTime + GetNetworkSendInterval())
                {
                    CmdMovedClick(agent.destination, agent.stoppingDistance);
                    lastSentDestination = agent.destination;
                    lastSentTime = Time.time;
                }
            }
            // wasd movement and velocity changed since last sync?
            // then send the latest POSITION (not velocity)
            // why is velocity not zero after stopping?
            else if (!hasPath && agent.velocity != Vector3.zero && transform.position != lastSentPosition)
            {
                // don't send all the time to not DDOS the server
                // note: if we check time BEFORE detecting move then moves are
                //       detected very late and cause additional latency and
                //       might cause move->idle->move effect if second move
                //       doesn't come in fast enough, etc.
                if (Time.time > lastSentTime + GetNetworkSendInterval())
                {
                    CmdMovedWASD(transform.position);
                    lastSentPosition = transform.position;
                    lastSentTime = Time.time;
                }
            }
        }
    }

    [TargetRpc]
    void TargetResetMovement(NetworkConnection conn)
    {
        // reset path and velocity
        agent.ResetMovement();
    }

    [TargetRpc]
    void TargetSetDestination(NetworkConnection conn, Vector3 destination, float stoppingDistance)
    {
        agent.stoppingDistance = stoppingDistance;
        agent.destination = destination;
    }

    // server-side serialization
    // used for the server to broadcast positions to other clients too
    public override bool OnSerialize(NetworkWriter writer, bool initialState)
    {
        // always send position so client knows if he's too far off and needs warp
        // we also need it for wasd movement anyway
        writer.Write(transform.position);

        // always send speed in case it's modified by something
        writer.Write(agent.speed);

        // click movement? then also send destination and stopping distance
        // (no need to send everything all the time, saves bandwidth)
        bool hasPath = agent.hasPath || agent.pathPending;
        writer.Write(hasPath);
        if (hasPath)
        {
            // destination
            writer.Write(agent.destination);

            // always send stopping distance because monsters might stop early etc.
            writer.Write(agent.stoppingDistance);
        }

        return true;
    }

    // client-side deserialization
    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        // read position, speed, movement type in any case, so that we read
        // exactly what we write
        Vector3 position = reader.ReadVector3();
        agent.speed = reader.ReadSingle();
        bool hasPath = reader.ReadBoolean();

        // click or wasd movement?
        if (hasPath)
        {
            // read destination and stopping distance
            Vector3 destination = reader.ReadVector3();
            float stoppingDistance = reader.ReadSingle();

            // ignore for local player since he can move freely
            if (!isLocalPlayer)
            {
                // try setting destination if on navmesh
                // (might not be while falling from the sky after joining etc.)
                if (agent.isOnNavMesh)
                {
                    agent.stoppingDistance = stoppingDistance;
                    agent.destination = destination;
                }
                else Debug.LogWarning("NetworkNavMeshAgent.OnSerialize: agent not on NavMesh, name=" + name + " position=" + transform.position + " destination=" + destination);
            }
        }
        else
        {
            // ignore for local player since he can move freely
            if (!isLocalPlayer)
            {
                // set position via .destination to get free interpolation
                agent.stoppingDistance = 0;
                agent.destination = position;
            }
        }

        // rubberbanding: if we are too far off because of a rapid position
        // change or latency or server side teleport, then warp
        // -> agent moves 'speed' meter per seconds
        // -> if we are speed * 2 units behind, then we teleport
        //    (using speed is better than using a hardcoded value)
        // -> we use speed * 2 for update/network latency tolerance. player
        //    might have moved quit a bit already before OnSerialize was called
        //    on the server.
        if (Vector3.Distance(transform.position, position) > agent.speed * 2 && agent.isOnNavMesh)
        {
            agent.Warp(position);
            //Debug.Log(name + " rubberbanding to " + position);
        }
    }
}
