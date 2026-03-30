namespace Core.Interface
{
    internal interface IExecute : IPresenter
    {
        public void Execute(float deltaTime);
    }
}