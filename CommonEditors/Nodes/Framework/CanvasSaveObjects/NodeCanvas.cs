using UnityEngine;
using System.Collections.Generic;

namespace CommonEditors.Nodes.Framework.CanvasSaveObjects
{
	public class NodeCanvas : ScriptableObject 
	{ // Just contains the nodes and global canvas stuff; an associated NodeEditorState holds the actual state now
		public List<Node> nodes = new List<Node> ();

		// current states in the state system
		public Node currentNode;
		public Transition currentTransition;
	}
}