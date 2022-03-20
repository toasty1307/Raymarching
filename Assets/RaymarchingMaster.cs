using UnityEngine;

namespace Raymarching2
{
    public class RaymarchingMaster : MonoBehaviour
    {
        public ComputeShader Shader;

        private RenderTexture _renderTexture;
        private Vector4 _direction;
        private Vector4 _pointLocation;

        private void Start()
        {
            _renderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBFloat)
            {
                enableRandomWrite = true
            };
            _renderTexture.Create();
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            if (_renderTexture.width != Screen.width || _renderTexture.height != Screen.height)
            {
                _renderTexture.Release();
                _renderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBFloat)
                {
                    enableRandomWrite = true
                };
                _renderTexture.Create();
            }
            Shader.SetTexture(0, "result", _renderTexture);
            Shader.SetVector("point_location", _pointLocation);
            Shader.SetVector("direction", _direction);
            Shader.Dispatch(0, Mathf.CeilToInt(Screen.width / 8f), Mathf.CeilToInt(Screen.height / 8f), 1);
            Graphics.Blit(_renderTexture, dest);
        }

        private void FixedUpdate()
        {
            _pointLocation += new Vector4(Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0, Input.GetKey(KeyCode.S) ? -1 : Input.GetKey(KeyCode.W) ? 1 : 0, 0, 0) * 10f;
            _direction += new Vector4(Input.GetKey(KeyCode.LeftArrow) ? -1 : Input.GetKey(KeyCode.RightArrow) ? 1 : 0, Input.GetKey(KeyCode.DownArrow) ? -1 : Input.GetKey(KeyCode.UpArrow) ? 1 : 0, 0, 0) * 100f;
        }
    }
}
