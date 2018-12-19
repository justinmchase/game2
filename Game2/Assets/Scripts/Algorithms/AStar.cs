using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Algorithms {
    public static class AStar {
        private static Vector3Int[] ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current) {
            var path = new List<Vector3Int>() { current };
            while (cameFrom.ContainsKey(current)) {
                current = cameFrom[current];
                path.Add(current);
            }

            path.Reverse();
            return path.ToArray();
        }

        private static float Heuristic(Vector3 p1, Vector3 p2) {
            return Vector3.Distance(p1, p2);
        }

        private static IEnumerable<Vector3Int> Neighbors(HashSet<Vector3Int> open, HashSet<Vector3Int> obstructed, Vector3Int p) {
            var u = p + new Vector3Int(0, 1, 0);
            var r = p + new Vector3Int(1, 0, 0);
            var l = p + new Vector3Int(-1, 0, 0);
            var d = p + new Vector3Int(0, -1, 0);

            var ul = p + new Vector3Int(-1, 1, 0);
            var ur = p + new Vector3Int(1, 1, 0);
            var lr = p + new Vector3Int(1, -1, 0);
            var ll = p + new Vector3Int(-1, -1, 0);

            if (open.Contains(u)) yield return u;
            if (open.Contains(d)) yield return d;
            if (open.Contains(l))
            {
                yield return l;
                if (open.Contains(ul) && !obstructed.Contains(u)) yield return ul;
                if (open.Contains(ll) && !obstructed.Contains(d)) yield return ll;
            }
            if (open.Contains(r))
            {
                yield return r;
                if (open.Contains(ur) && !obstructed.Contains(u)) yield return ur;
                if (open.Contains(lr) && !obstructed.Contains(d)) yield return lr;
            }
        }

        public static Vector3Int[] GetPath(
            HashSet<Vector3Int> open,
            HashSet<Vector3Int> obstructed,
            Vector3Int start,
            Vector3Int goal) {
            // The set of nodes already evaluated
            var closedSet = new HashSet<Vector3Int>();

            // The set of currently discovered nodes that are not evaluated yet.
            // Initially, only the start node is known.
            var openSet = new HashSet<Vector3Int>() { start };

            // For each node, which node it can most efficiently be reached from.
            // If a node can be reached from many nodes, cameFrom will eventually contain the
            // most efficient previous step.
            var cameFrom = new Dictionary<Vector3Int, Vector3Int>();

            // For each node, the cost of getting from the start node to that node.
            var gScore = new Dictionary<Vector3Int, float> {
                { start, 0 }
            };

            // For each node, the total cost of getting from the start node to the goal
            // by passing by that node. That value is partly known, partly heuristic.
            var fScore = new Dictionary<Vector3Int, float> {
                { start, Heuristic(start, goal) }
            };

            while (openSet.Any()) {
                // the node in openSet having the lowest fScore[] value
                var current = fScore
                    .Where(x => openSet.Contains(x.Key))
                    .OrderBy(x => x.Value)
                    .First()
                    .Key;

                if (current == goal) {
                    return ReconstructPath(cameFrom, current);
                }

                openSet.Remove(current);
                closedSet.Add(current);
                foreach(var neighbor in Neighbors(open, obstructed, current)) {
                    if (closedSet.Contains(neighbor))
                        continue; // Ignore the neighbor which is already evaluated.

                    // The distance from start to a neighbor
                    var tentative_gScore = gScore[current] + Vector3Int.Distance(current, neighbor);

                    if (!openSet.Contains(neighbor)) { // Discover a new node
                        openSet.Add(neighbor);
                    } else if (tentative_gScore >= gScore[neighbor]) {
                        continue;
                    }

                    // This path is the best until now. Record it!
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentative_gScore;
                    fScore[neighbor] = Heuristic(neighbor, goal) + tentative_gScore;
                }
            }

            // Should not happen, could not find a valid path.
            return new Vector3Int[] { };
        }
    }
}
