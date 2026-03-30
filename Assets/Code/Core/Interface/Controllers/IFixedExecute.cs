namespace Core.Interface
{
    internal interface IFixedExecute : IPresenter
    {
        public void FixedExecute(float fixedDeltaTime);
    }
}