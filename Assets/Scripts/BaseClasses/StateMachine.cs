/*
 * Created by:
 * Name: James Sturdgess
 * Sid: 1314371
 * Date Created: 04/10/2019
 * Last Modified 04/10/2019
 * Modified By:
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class StateMachine<T>
    {
        public State<T> CurrentState { get; private set; }
        public T Parent;

        public StateMachine(T p)
        {

            Parent = p;
            CurrentState = null;

        }

        public void ChangeState(State<T> newState)
        {

            if (CurrentState != null)
                CurrentState.ExitState(Parent);
            CurrentState = newState;
            CurrentState.EnterState(Parent);

        }

        public void Update()
        {

            if (CurrentState != null)
                CurrentState.UpdateState(Parent);

        }

    }

    public abstract class State<T>
    {

        public abstract void EnterState(T parent);
        public abstract void ExitState(T parent);
        public abstract void UpdateState(T parent);

    }

}
