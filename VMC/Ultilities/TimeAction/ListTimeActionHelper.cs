using System;
using System.Collections.Generic;
using UnityEngine;

namespace VMC.Ultilities.TimeAction
{
    public class ListTimeActionHelper : SingletonAdvance<ListTimeActionHelper>
    {
        List<TimedAction> listActions = new List<TimedAction>();

        public void AddDelayAction(float delayInSeconds, Action actionToExecute, bool ignoreTimeScale = true, int loopTime = 1)
        {
            TimedAction newTimedAction = new TimedAction()
            {
                delayTime = delayInSeconds,
                executeTime = Time.time + delayInSeconds,
                countTime = delayInSeconds,
                action = actionToExecute,
                ignoreTimeScale = ignoreTimeScale,
                loopTimes = loopTime
            };
            listActions.Add(newTimedAction);
        }

        void Update()
        {
            if (listActions.Count != 0)
            {
                for (int i = listActions.Count - 1; i >= 0; i--)
                {
                    if (listActions[i].ignoreTimeScale)
                    {
                        if (Time.time >= listActions[i].executeTime)
                        {
                            listActions[i].action();
                            listActions[i].loopTimes -= 1;
                            if (listActions[i].loopTimes == 0)
                            {
                                listActions.RemoveAt(i);
                            }
                        }
                    }
                    else
                    {
                        listActions[i].countTime -= Time.deltaTime;
                        if (listActions[i].countTime <= 0)
                        {
                            listActions[i].action();
                            listActions[i].loopTimes -= 1;
                            if (listActions[i].loopTimes == 0)
                            {
                                listActions.RemoveAt(i);
                            }
                        }
                    }
                }

            }
        }

        public void Clear()
        {
            listActions.Clear();
        }
    }
}