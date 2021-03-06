﻿using ProblemSolving.BySearching.Strategy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using ProblemSolving.BySearching.Returns;

namespace ProblemSolving.BySearching.Strategy
{
    public class DepthFirstStrategy<TState, TAction> : ISearchStrategy<TState, TAction>
        where TState : IEquatable<TState>
    {
        private readonly EqualityComparer<Node<TState, TAction>> nComparer
            = NodeStateEqualityComparer<TState, TAction>.New();

        private readonly EqualityComparer<TState> sComparer
            = EqualityComparer<TState>.Default;

        public SolutionSearchBase<TState, TAction> Search(ISearchProblem<TState, TAction> problem)
        {
            var rootNode = Node<TState, TAction>.Root(problem.InitialState);

            if (problem.GoalTest(rootNode.State))
            {
                return new SolutionFound<TState, TAction>(rootNode);
            }


            var frontier = new Stack<Node<TState, TAction>>();

            var explored = new HashSet<TState>(sComparer);

            frontier.Push(rootNode);

            while (frontier.Count > 0)
            {
                var node = frontier.Pop();

                explored.Add(node.State);

                var actions = problem.Actions(node.State);

                foreach (var action in actions)
                {
                    var child = NodeExtensions.ChildNode(problem, node, action);

                    bool containsState = explored.Contains(child.State)
                        || frontier.Contains(child, nComparer);

                    if (!containsState)
                    {
                        if (problem.GoalTest(child.State))
                        {
                            return new SolutionFound<TState, TAction>(child);
                        }

                        frontier.Push(child);
                    }
                }
            }

            return new SolutionFailure<TState, TAction>();
        }
    }
}
