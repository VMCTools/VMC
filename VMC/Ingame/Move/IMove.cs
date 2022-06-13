namespace VMC.Ingame.Move
{
    public interface IMove
    {
        void Move();

        void Pause();
        void Resume();
        void Stop();

        bool IsComplete();
    }
}