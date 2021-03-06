﻿using ProblemSolving;
using ProblemSolving.BySearching.Agent;
using ProblemSolving.BySearching;
using System;
using AppEightPuzzle.Environment;

namespace AppEightPuzzle.Problem
{
    public class SearchAgent<T> : SimpleProblemSolvingAgentBase<ProblemState<T>, ProblemAction>
        where T : IComparable<T>
    {
        public SearchAgent(
            ISearchStrategy<ProblemState<T>, ProblemAction> searchStrategy)
            : base(searchStrategy)
        { }

        protected override ProblemState<T> UpdateState<TPercepted>(
            ProblemState<T> state, TPercepted percept)
        {
            var perceptedObj = percept as PuzzleBoard<T>;

            return perceptedObj.ToProblemState();
        }

        protected override ProblemState<T> FormulateGoal(ProblemState<T> state)
        {
            var values = state.ToValues();

            Array.Sort(values);

            return values.ToProblemState(state.GetLength(0), state.GetLength(1));
        }

        protected override ISearchProblem<ProblemState<T>, ProblemAction> FormulateProblem(
            ProblemState<T> state, ProblemState<T> goal)
        {
            return new ProblemDefinition<T>()
            {
                InitialState = state,
                GoalState = goal
            };
        }
    }
}
