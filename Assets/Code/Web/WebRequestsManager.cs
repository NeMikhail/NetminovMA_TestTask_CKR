using System;
using Core.Interface;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using Zenject;

namespace Web
{
    public class WebRequestsManager : IPreInitialisation, IFixedExecute, ICleanUp
    {
        private WebRequestsQueue _webRequestsQueue;
        private bool _isBusy;
        private UnityWebRequest _currentRequest;
        private WebRequestToken _currentToken;

        [Inject]
        public void Construct(WebRequestsQueue webRequestsQueue)
        {
            _webRequestsQueue = webRequestsQueue;
        }

        public void PreInitialisation()
        {
            _isBusy = false;
        }

        public void FixedExecute(float fixedDeltaTime)
        {
            if (!_isBusy && !_webRequestsQueue.IsEmpty())
            {
                ProcessRequest().Forget();
            }
        }

        public void Cleanup()
        {
            if (_currentRequest != null)
            {
                _currentToken?.Cancel();
                _currentRequest.Abort();
                _currentRequest = null;
            }
            _webRequestsQueue.Clear();
        }

        private async UniTaskVoid ProcessRequest()
        {
            _isBusy = true;
            var (request, token) = _webRequestsQueue.Dequeue();

            if (token.IsCancelled)
            {
                request.Abort();
                request.Dispose();
                _isBusy = false;
                return;
            }

            _currentRequest = request;
            _currentToken = token;

            try
            {
                await request.SendWebRequest();

                if (!token.IsCancelled && !request.isNetworkError)
                    token.Complete(request.downloadHandler.text);
                else
                    token.Cancel();
            }
            catch (Exception)
            {
                token.Cancel();
            }
            finally
            {
                request.Dispose();
                _currentRequest = null;
                _currentToken = null;
                _isBusy = false;
            }
        }
    }
}
