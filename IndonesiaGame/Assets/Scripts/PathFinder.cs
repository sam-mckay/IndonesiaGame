using UnityEngine;
using System.Collections.Generic;

public class PathFinder
{
    private struct AStarNode : System.IEquatable<AStarNode>
    {
        public Vector2 at;

        public float gScore;
        public float hScore;
        public float fScore;

        public Vector2 cameFrom;

        public bool Equals(AStarNode to)
        {
            return this.at == to.at;
        }
    }

    private List<AStarNode> _openSet;
    private List<AStarNode> _closedSet;

    public float GridPathfind(Vector2 start, Vector2 end, int width, int height, int[,] map)
    {
        _openSet = new List<AStarNode>();
        _closedSet = new List<AStarNode>();

        if (start == end)
            return 0.0f;

        AStarNode startNode = new AStarNode();
        startNode.at = start;
        startNode.gScore = 0.0f;
        startNode.hScore = CalculateHeuristic(start, end);
        startNode.fScore = startNode.gScore + startNode.hScore;

        _openSet.Add(startNode);

        while (_openSet.Count > 0)
        {
            AStarNode currentNode = _openSet[0];

            _openSet.Remove(currentNode);
            _closedSet.Add(currentNode);

            if (currentNode.at == end)
            {
                AStarNode nodeToCheck = new AStarNode();
                nodeToCheck.at = end;
                nodeToCheck = _closedSet[_closedSet.IndexOf(nodeToCheck)];

                float counter = 0.0f;

                while (nodeToCheck.at != start)
                {
                    counter += 1.0f;
                    AStarNode temp = new AStarNode();
                    temp.at = nodeToCheck.cameFrom;
                    nodeToCheck = _closedSet[_closedSet.IndexOf(temp)];
                }

                return counter;
            }

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 ^ y == 0)
                    {
                        int checkX = (int)(x + currentNode.at.x);
                        int checkY = (int)(y + currentNode.at.y);

                        if (ReadMap(checkX, checkY, width, height, map))
                        {
                            if (IsInClosed(checkX, checkY))
                                continue;

                            AStarNode tempNode = new AStarNode();
                            tempNode.gScore = currentNode.gScore + 1;
                            tempNode.at = new Vector2(checkX, checkY);

                            if (!IsInOpen(checkX, checkY))
                            {

                                tempNode.hScore = CalculateHeuristic(currentNode.at, end);
                                tempNode.fScore = tempNode.gScore + tempNode.hScore;
                                tempNode.cameFrom = currentNode.at;

                                if (_openSet.Count == 0)
                                    _openSet.Add(tempNode);

                                int counter = 0;
                                while (counter != _openSet.Count && tempNode.fScore > _openSet[counter].fScore)
                                    counter++;

                                _openSet.Insert(counter, tempNode);
                            }
                            else if (tempNode.gScore > _openSet[_openSet.IndexOf(tempNode)].gScore)
                            {
                                break;
                            }
                            else
                            {
                                int idx = _openSet.IndexOf(tempNode);
                                AStarNode temp = _openSet[idx];
                                temp.gScore = tempNode.gScore;
                                temp.fScore = temp.gScore + tempNode.hScore;
                                temp.cameFrom = currentNode.at;
                                _openSet[idx] = temp;
                            }
                        }
                    }
                }
            }
        }
        return -1.0f;

    }

    private float CalculateHeuristic(Vector2 start, Vector2 target)
    {
        return Mathf.Abs(start.x - target.x + start.y - target.y);
    }

    private bool IsInMapRange(int x, int y, int width, int height)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    private bool ReadMap(int x, int y, int width, int height, int[,] map)
    {
        if (IsInMapRange(x, y, width, height))
        {
            if (map[x, y] == 0)
                return true;
            else
                return false;
        }
        return false;
    }

    private bool IsInOpen(int x, int y)
    {
        foreach (AStarNode point in _openSet)
        {
            if (point.at.x == x && point.at.y == y)
                return true;
        }
        return false;
    }
    private bool IsInClosed(int x, int y)
    {
        foreach (AStarNode point in _closedSet)
        {
            if (point.at.x == x && point.at.y == y)
                return true;
        }
        return false;
    }
}
