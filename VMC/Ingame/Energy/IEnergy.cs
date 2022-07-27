
namespace VMC.Ingame.Energy
{
    public interface IEnergy
    {
        /// <summary>
        /// init the value of energy
        /// </summary>
        void Init(float startValue, float maxValue);

        /// <summary>
        /// sub energy by value
        /// </summary>
        /// <param name="value"> value </param>
        void Sub(float value);

        /// <summary>
        /// Restore value by percent
        /// </summary>
        /// <param name="percent"> percent refill [0,1]</param>
        void Revive(float percent);
    }
}