using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDesigner
{
    public enum ConnectionPortType { None,In,Out}
    public enum ConnectionPortMode { Single,Multi,Max}
    public abstract class ConnectionPort
    {
        public ConnectionPortType portType;
        public ConnectionPortMode mode;
        public int maxConnectionCount;
        public ConnectionPort toPort;
        public List<Connection> connections;
        public GraphNode selfNode;
        public virtual bool CanContect(GraphNode node,ConnectionPort targetPort)
        {
            return false;
        }

        public virtual void ApplyContect(GraphNode node, ConnectionPort targetPort)
        {
            
        }

    }

}
