using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//#pragma warning disable 0168 // variable declared but not used.
//#pragma warning disable 0219 // variable assigned but not used.
//#pragma warning disable 0414 // private field assigned but not used.

namespace TDTK {

	public delegate void SetPathCallbackTD(List<Vector3> wp);
	
	
	public class AStar : MonoBehaviour {
		
		private static AStar instance;
		
		[Tooltip("Check to enable flying creep to bypass any obstacle\nWhen enabled, flying creep will always cut a straight-line through platform and move through shortest path even when it's are blocked")]
		public bool flyingBypass=false;
		public static bool EnableFlyingBypass(){ return instance!=null ? instance.flyingBypass : true; }
		
		
		[Tooltip("Check to enable path smoothing, creep will try to cut diagonally when possible")]
		public bool pathSmoothing=false;
		public static bool EnablePathSmoothing(){ return instance!=null ? instance.pathSmoothing : false; }
		
		
		
		public void Awake(){
			if(instance!=null) return;
			instance=this;
		}
		
		
		public static void Init(){
			if(instance!=null) return;
			
			instance = (AStar)FindObjectOfType(typeof(AStar));
			
			if(instance==null){
				GameObject obj=new GameObject();
				instance=obj.AddComponent<AStar>();
				obj.name="AStar";
			}
		}
		
		
		public static void ConnectPlatform(BuildPlatform plat1, BuildPlatform plat2, List<Vector3> wpList){
			
		}
		
		
		private static Transform refT;
		public static NodeTD[] GenerateNode(BuildPlatform platform, float gridSize=1, float heightOffset=0){
			if(refT==null){
				refT=new GameObject("RefT").transform;
				refT.parent=TowerManager.GetInstance().transform;
			}
			
			Transform platformT=platform.transform;
			
			float scaleX=platformT.localScale.x;
			float scaleZ=platformT.localScale.y;
			
			int countX=(int)(scaleX/gridSize);
			int countZ=(int)(scaleZ/gridSize);
			
			
			float x=-scaleX/2/scaleX;
			float z=-scaleZ/2/scaleZ;
			
			Vector3 point=platformT.TransformPoint(new Vector3(x, z, 0));
			
			refT.position=point;
			refT.rotation=platformT.rotation*Quaternion.Euler(-90, 0, 0);
			
			refT.position=refT.TransformPoint(new Vector3(gridSize/2, heightOffset, gridSize/2));
			
			NodeTD[] nodeGraph=new NodeTD[countZ*countX];
			
			int counter=0;
			for(int i=0; i<countZ; i++){
				for(int j=0; j<countX; j++){
					nodeGraph[counter]=new NodeTD(refT.position, counter);
					counter+=1;
					refT.position=refT.TransformPoint(new Vector3(gridSize, 0, 0));
				}
				refT.position=refT.TransformPoint(new Vector3(-(countX)*gridSize, 0, gridSize));
			}
			
			refT.position=Vector3.zero;
			refT.rotation=Quaternion.identity;
			
			LayerMask mask=1<<TDTK.GetLayerPlatform() | 1<<TDTK.GetLayerTower() | 1<<TDTK.GetLayerTerrain() | 1<<TDTK.GetLayerNoBuild();
			LayerMask maskTowerBlock=1<<TDTK.GetLayerNoBuild();
			
			counter=0;
			foreach(NodeTD cNode in nodeGraph){
				//check if there's anything within the point
				Collider[] cols=Physics.OverlapSphere(cNode.pos, gridSize*0.45f, ~mask);
				if(cols.Length>0){ cNode.SetWalkable(false); counter+=1; }
				
				cols=Physics.OverlapSphere(cNode.pos, gridSize*0.45f, maskTowerBlock);
				if(cols.Length>0){ cNode.SetBlockedForTower(true); }
			}
			
			
			float neighbourDistance=0;
			float neighbourRange=gridSize*1.1f;
			//if(instance.connectDiagonalNeighbour) neighbourRange=gridSize*1.5f;
			//else neighbourRange=gridSize*1.1f;
			
			counter=0;
			//assign the neighouring  node for each node in the grid
			foreach(NodeTD currentNode in nodeGraph){
				//only if that node is walkable
				if(currentNode.IsWalkable()){
				
					//create an empty array
					List<NodeTD> neighbourNodeList=new List<NodeTD>();
					List<float> neighbourCostList=new List<float>();
					
					NodeTD[] neighbour=new NodeTD[8];
					int id=currentNode.ID;
					
					if(id>countX-1 && id<countX*countZ-countX){
						//print("middle rows");
						if(id!=countX) neighbour[0]=nodeGraph[id-countX-1];
						neighbour[1]=nodeGraph[id-countX];
						neighbour[2]=nodeGraph[id-countX+1];
						neighbour[3]=nodeGraph[id-1];
						neighbour[4]=nodeGraph[id+1];
						neighbour[5]=nodeGraph[id+countX-1];
						neighbour[6]=nodeGraph[id+countX];
						if(id!=countX*countZ-countX-1)neighbour[7]=nodeGraph[id+countX+1];
					}
					else if(id<=countX-1){
						//print("first row");
						if(id!=0) neighbour[0]=nodeGraph[id-1];
						if(nodeGraph.Length>id+1) neighbour[1]=nodeGraph[id+1];
						if(countZ>0){
							if(nodeGraph.Length>id+countX-1)	neighbour[2]=nodeGraph[id+countX-1];
							if(nodeGraph.Length>id+countX)		neighbour[3]=nodeGraph[id+countX];
							if(nodeGraph.Length>id+countX+1)	neighbour[4]=nodeGraph[id+countX+1];
						}
					}
					else if(id>=countX*countZ-countX){
						//print("last row");
						neighbour[0]=nodeGraph[id-1];
						if(id!=countX*countZ-1) neighbour[1]=nodeGraph[id+1];
						if(id!=countX*(countZ-1))neighbour[2]=nodeGraph[id-countX-1];
						neighbour[3]=nodeGraph[id-countX];
						neighbour[4]=nodeGraph[id-countX+1];
					}
					


					//scan through all the node in the grid
					foreach(NodeTD node in neighbour){
						//if this the node is not currentNode
						if(node!=null && node.IsWalkable()){
							//if this node is within neighbour node range
							neighbourDistance=GetHorizontalDistance(currentNode.pos, node.pos);
							if(neighbourDistance<neighbourRange){
								//if nothing's in the way between these two
								if(!Physics.Linecast(currentNode.pos, node.pos, ~mask)){
									//if the slop is not too steep
									//if(Mathf.Abs(GetSlope(currentNode.pos, node.pos))<=maxSlope){
										//add to list
										//if(!node.walkable) Debug.Log("error");
										neighbourNodeList.Add(node);
										neighbourCostList.Add(neighbourDistance);
									//}//else print("too steep");
								}//else print("something's in the way");
							}//else print("out of range "+neighbourDistance);
						}//else print("unwalkable");
					}

					//set the list as the node neighbours array
					currentNode.SetNeighbour(neighbourNodeList, neighbourCostList);
					
					//if(neighbourNodeList.Count==0)
						//Debug.Log("no heighbour. node number "+counter+"  "+neighbourNodeList.Count);
				}
				
				counter+=1;
			}
			
			return nodeGraph;
		}
		
		public static float GetHorizontalDistance(Vector3 p1, Vector3 p2){
			p1.y=0;	p2.y=0;
			return Vector3.Distance(p1, p2);
		}
		
		
		
		
		//searchMode:  0-any node, 1-walkable only, 2-unwalkable only
		//public static NodeTD GetNearestNode(Vector3 point, NodeTD[] graph){ return GetNearestNode(point, graph, 0); }
		public static NodeTD GetNearestNode(Vector3 point, NodeTD[] graph, int searchMode=0){
			float dist=Mathf.Infinity;
			float currentNearest=Mathf.Infinity;
			NodeTD nearestNode=null;
			
			if(searchMode==0){
				foreach(NodeTD node in graph){
					dist=Vector3.Distance(point, node.pos);
					if(dist<currentNearest){ currentNearest=dist; nearestNode=node; }
				}
			}
			else if(searchMode==1){
				foreach(NodeTD node in graph){
					if(!node.IsWalkable()) continue;
					dist=Vector3.Distance(point, node.pos);
					if(dist<currentNearest){ currentNearest=dist; nearestNode=node; }
				}
			}
			else if(searchMode==2){
				foreach(NodeTD node in graph){
					if(node.IsWalkable()) continue;
					dist=Vector3.Distance(point, node.pos);
					if(dist<currentNearest){ currentNearest=dist; nearestNode=node; }
				}
			}
			
			return nearestNode;
		}
		
		
		
		
		
		
		//make cause system to slow down, use with care
		public static List<Vector3> Search(NodeTD startN, NodeTD endN, NodeTD[] graph, NodeTD blockN=null, int footprint=-1){
			Init();
			
			if(startN.IsBlocked()) return new List<Vector3>();
			
			if(blockN!=null) blockN.SetWalkable(false);
			
			bool pathFound=true;
			
			int searchCounter=0;	//used to count the total amount of node that has been searched
			
			List<NodeTD> closeList=new List<NodeTD>();
			NodeTD[] openList=new NodeTD[graph.Length];
			
			List<int> openListRemoved=new List<int>();
			int openListCounter=0;

			NodeTD currentNode=startN;
			
			float currentLowestF=Mathf.Infinity;
			int id=0;	//use element num of the node with lowest score in the openlist during the comparison process
			int i=0;		//universal int value used for various looping operation
			
			while(true){
				if(currentNode==endN) break;
				closeList.Add(currentNode);
				currentNode.listState=_ListStateTD.Close;
				
				currentNode.ProcessNeighbour(endN);
				foreach(NodeTD neighbour in currentNode.neighbourNode){
					if(neighbour.listState==_ListStateTD.Unassigned && !neighbour.IsBlocked()) {
						neighbour.listState=_ListStateTD.Open;
						if(openListRemoved.Count>0){
							openList[openListRemoved[0]]=neighbour;
							openListRemoved.RemoveAt(0);
						}
						else{
							openList[openListCounter]=neighbour;
							openListCounter+=1;
						}
					}
				}
				
				currentNode=null;
				
				currentLowestF=Mathf.Infinity;
				id=0;
				for(i=0; i<openListCounter; i++){
					if(openList[i]!=null){
						if(openList[i].scoreF<currentLowestF){
							currentLowestF=openList[i].scoreF;
							currentNode=openList[i];
							id=i;
						}
					}
				}
				
				if(currentNode==null) {
					pathFound=false;
					break;
				}
				
				openList[id]=null;
				openListRemoved.Add(id);

				searchCounter+=1;
			}
			
			
			List<Vector3> p=new List<Vector3>();
			if(pathFound){
				if(EnablePathSmoothing()){
					List<NodeTD> pn=new List<NodeTD>();
					while(currentNode!=null){
						pn.Add(currentNode);
						currentNode=currentNode.parent;
					}
					
					//for(int n=0; n<pn.Count; n++) Debug.DrawLine(pn[n].GetPos(), pn[n].GetPos()+new Vector3(0, .5f, 0), Color.white, .5f);
					pn=PathSmoothing(pn);
					for(int n=0; n<pn.Count; n++) p.Add(pn[n].GetPos());
					//for(int n=0; n<pn.Count; n++) Debug.DrawLine(pn[n].GetPos(), pn[n].GetPos()+new Vector3(0, .25f, 0), Color.red, .5f);
				}
				else{
					while(currentNode!=null){
						p.Add(currentNode.pos);
						currentNode=currentNode.parent;
					}
				}
				p.Reverse();
			}
			
			if(blockN!=null) blockN.SetWalkable(true); 
			
			ResetGraph(graph);
			
			return p;
		}
		
		
		
		
		public static List<NodeTD> PathSmoothing(List<NodeTD> srcPath){
			for(int i=0; i<srcPath.Count-2; i++){
				Vector3 dir=srcPath[i].GetPos()-srcPath[i+2].GetPos();
				if(dir.x*dir.z==0) continue;
				
				NodeTD[] neighbourList1=srcPath[i].neighbourNode;
				NodeTD[] neighbourList2=srcPath[i+2].neighbourNode;
				List<NodeTD> commonNeighbourList=new List<NodeTD>();
				
				for(int n=0; n<neighbourList1.Length; n++){
					for(int m=0; m<neighbourList2.Length; m++){
						if(neighbourList1[n]==neighbourList2[m]){
							if(!commonNeighbourList.Contains(neighbourList2[m])) commonNeighbourList.Add(neighbourList2[m]);
						}
					}
				}
				
				bool blocked=false;
				if(commonNeighbourList.Count>1){
					for(int n=0; n<commonNeighbourList.Count; n++){
						if(!commonNeighbourList[n].IsWalkable() || commonNeighbourList[n].IsOccupied()){
							blocked=true; 	break;
						}
					}
				}
				else blocked=true;
				
				if(blocked) continue;
					
				srcPath.RemoveAt(i+1);
			}
			return srcPath;
		}
		
		
		
		public static void ResetGraph(NodeTD[] nodeGraph){
			foreach(NodeTD node in nodeGraph){
				node.listState=_ListStateTD.Unassigned;
				node.parent=null;
			}
		}
		
		
	}
	
	
	
	public enum _ListStateTD{Unassigned, Open, Close};
	public class NodeTD{
		public int ID;
		public Vector3 pos;
		public NodeTD[] neighbourNode;
		public float[] neighbourCost;
		public NodeTD parent;
		private bool walkable=true;
		public float scoreG;
		public float scoreH;
		public float scoreF;
		public _ListStateTD listState=_ListStateTD.Unassigned;
		public float tempScoreG=0;
		
		private bool blockedForTower=false;
		private UnitTower occupiedTower;
		
		public NodeTD(){}
		
		public NodeTD(Vector3 position, int id){
			pos=position;
			ID=id;
		}
		
		public Vector3 GetPos(){ return pos; }
		
		public UnitTower GetTower(){ return occupiedTower; }
		public void ClearTower(){ occupiedTower=null; }
		public void SetTower(UnitTower t){
			if(occupiedTower!=null) Debug.LogWarning("Node has been occupied!"); 
			occupiedTower=t;
		}
		
		
		
		public void SetWalkable(bool flag){ walkable=flag; }
		public bool IsWalkable(){ return walkable; }
		
		public void SetBlockedForTower(bool flag){ blockedForTower=flag; }
		public bool IsBlockedForTower(){ return !walkable || blockedForTower; }
		
		//public bool IsBlocked1(){ return walkable & (occupiedTower==null || occupiedTower.IsMine()); }
		public bool IsBlocked(){ return !walkable || (occupiedTower!=null && !occupiedTower.IsMine()); }
		public bool IsOccupied(){ return occupiedTower!=null; }
		
		public void SetNeighbour(List<NodeTD> arrNeighbour, List<float> arrCost){
			neighbourNode = arrNeighbour.ToArray();
			neighbourCost = arrCost.ToArray();
		}
		
		public void ProcessNeighbour(NodeTD node){
			ProcessNeighbour(node.pos);
		}
		
		//call during a serach to scan through all neighbour, check their score against the position passed
		public void ProcessNeighbour(Vector3 pos){
			for(int i=0; i<neighbourNode.Length; i++){
				//if the neightbour state is clean (never evaluated so far in the search)
				if(neighbourNode[i].listState==_ListStateTD.Unassigned){
					//check the score of G and H and update F, also assign the parent to currentNode
					neighbourNode[i].scoreG=scoreG+neighbourCost[i];
					neighbourNode[i].scoreH=Vector3.Distance(neighbourNode[i].pos, pos);
					neighbourNode[i].UpdateScoreF();
					neighbourNode[i].parent=this;
				}
				//if the neighbour state is open (it has been evaluated and added to the open list)
				else if(neighbourNode[i].listState==_ListStateTD.Open){
					//calculate if the path if using this neighbour node through current node would be shorter compare to previous assigned parent node
					tempScoreG=scoreG+neighbourCost[i];
					if(neighbourNode[i].scoreG>tempScoreG){
						//if so, update the corresponding score and and reassigned parent
						neighbourNode[i].parent=this;
						neighbourNode[i].scoreG=tempScoreG;
						neighbourNode[i].UpdateScoreF();
					}
				}
			}
		}
		
		void UpdateScoreF(){
			scoreF=scoreG+scoreH;
		}
		
	}

	

}