using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VMC.Ultilities.TimeAction
{
    public class QueueTimeActionHelper : SingletonAdvance<QueueTimeActionHelper>
    {
        List<TimedAction> queuedActions = new List<TimedAction>();

        public void QueueAction(float delayInSeconds, Action actionToExecute, bool ignoreTimeScale = true, int loopTime = 1)
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

            for (int x = 0; x < queuedActions.Count; x++)
            {

                if (newTimedAction.executeTime < queuedActions[x].executeTime)
                {
                    queuedActions.Insert(x, newTimedAction);

                    return;
                }
            }

            queuedActions.Add(newTimedAction);
        }

        void Update()
        {
            while (queuedActions.Count > 0)
            {
                if (queuedActions[0].ignoreTimeScale)
                {
                    if (Time.time >= queuedActions[0].executeTime)
                    {
                        queuedActions[0].action();
                        queuedActions[0].loopTimes -= 1;
                        if (queuedActions[0].loopTimes == 0)
                        {
                            queuedActions.RemoveAt(0);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    queuedActions[0].countTime -= Time.deltaTime;
                    if (queuedActions[0].countTime <= 0)
                    {
                        queuedActions[0].action();
                        queuedActions[0].loopTimes -= 1;
                        if (queuedActions[0].loopTimes == 0)
                        {
                            queuedActions.RemoveAt(0);
                        }
                    }
                }

            }
        }

        public void Clear()
        {
            queuedActions.Clear();
        }
    }
}