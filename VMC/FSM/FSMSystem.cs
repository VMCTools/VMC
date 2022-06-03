namespace VMC.FSM
{
    public class FSMSystem
    {
        public FSMState currentState { set; get; }
        // Start is called before the first frame update

        // Update is called once per frame
        public void Update()
        {
            if (currentState != null)
            {
                currentState.OnUpdate();
            }
            OnSystemUpdate();
        }
        public void GotoState(FSMState newState)
        {
            if (currentState != null)
            {
                currentState.OnExit();
            }
            // TODO: For debug monster.
            //if (this is MonsterFSMSystem)
            //{
            //    Debug.Log($"MONSTER Changes state: {currentState.ToString()} ----->>> {newState.ToString()}");
            //}
            currentState = newState;
            currentState.OnEnter();
        }
        public void GotoState(FSMState newState, object data)
        {
            if (currentState != null)
            {
                currentState.OnExit();
            }
            currentState = newState;
            currentState.OnEnter(data);
        }
        public virtual void OnSystemUpdate()
        {

        }
        public virtual void OnSystemLateUpdate()
        {

        }
        public virtual void OnSystemFixedUpdate()
        {

        }
        public void OnMiddleAnim()
        {
            currentState.OnEventMiddleAnimation();
        }
        public void OnEndAnim()
        {
            currentState.OnEventEndAnimation();
        }
    }
}