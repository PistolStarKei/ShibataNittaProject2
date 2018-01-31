using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestNode : MonoBehaviour {

	public enum NodeStatus{None,Open,Closed};
	public class Node{

		public Vector2 pos;
		public NodeStatus status;
		public int cost;
		public int hulisticCost;
		public Node parentNode;
	}

	public void Hantei(Vector2 goal){
		Node startNode=new Node();
		startNode.pos=new Vector2(1,1);
		startNode.cost=0;

		//int x=goal.x-startNode.pos.x;
		//int y=goal.y-startNode.pos.y;

		//startNode.hulisticCost=x>=y?x:y;


		Debug.Log("スコア = " +(startNode.cost+startNode.hulisticCost));

	}

	/*
	public void GetPath(List<Vector2> pList) {
		// リストに座標情報を追加していく
		pList.Add(new Vector2(X, Y));
		if(_parent != null) {
			// 再帰的に親ノードをたどる
			_parent.GetPath(pList);
		}
	}

	public Node SearchMinScoreNodeFromOpenList() {
		// 最小スコア
		int min = 9999;
		// 最小実コスト
		int minCost = 9999;
		Node minNode = null;
		foreach(Node node in _openList) {
			int score = node.GetScore();
			if(score > min) {
				// スコアが大きい
				continue;
			}
			if(score == min && node.cost >= minCost) {
				// スコアが同じときは実コストも比較する
				continue;
			}

			// 最小値更新.
			min = score;
			minCost = node.cost;
			minNode = node;
		}
		return minNode;
	}*/


}
