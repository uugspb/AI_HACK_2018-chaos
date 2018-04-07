using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraProjectionChange : MonoBehaviour
{    
    [SerializeField] private float ProjectionChangeTime = 0.5f;

    private bool ChangeProjection = false;
    private bool _changing = false;
    private float _currentT = 0.0f;
    private Camera _camera;

    public bool IsOrthographic { get { return _camera.orthographic; } }

    [ContextMenu("SetOrthographic")]
    public void SetOrthographic()
    {
        if(!_changing && _camera.orthographic == false)
        {
            ChangeProjection = true;
        }
    }

    [ContextMenu("SetPerspective")]
    public void SetPerspective()
    {
        if (!_changing && _camera.orthographic == true)
        {
            ChangeProjection = true;
        }
    }

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (_changing)
        {
            ChangeProjection = false;
        }
        else if (ChangeProjection)
        {
            _changing = true;
            _currentT = 0.0f;
        }
    }

    private void LateUpdate()
    {
        if (!_changing)
        {
            return;
        }

        var currentlyOrthographic = _camera.orthographic;
        Matrix4x4 orthoMat, persMat;
        if (currentlyOrthographic)
        {
            orthoMat = _camera.projectionMatrix;

            _camera.orthographic = false;
            _camera.ResetProjectionMatrix();
            persMat = _camera.projectionMatrix;
        }
        else
        {
            persMat = _camera.projectionMatrix;

            _camera.orthographic = true;
            _camera.ResetProjectionMatrix();
            orthoMat = _camera.projectionMatrix;
        }
        _camera.orthographic = currentlyOrthographic;

        _currentT += (Time.deltaTime / ProjectionChangeTime);
        if (_currentT < 1.0f)
        {
            if (currentlyOrthographic)
            {
                _camera.projectionMatrix = MatrixLerp(orthoMat, persMat, _currentT );
            }
            else
            {
                _camera.projectionMatrix = MatrixLerp(persMat, orthoMat, _currentT );
            }
        }
        else
        {
            _changing = false;
            _camera.orthographic = !currentlyOrthographic;
            _camera.ResetProjectionMatrix();
        }
    }

    private Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float t)
    {
        t = Mathf.Clamp(t, 0.0f, 1.0f);
        var newMatrix = new Matrix4x4();
        newMatrix.SetRow(0, Vector4.Lerp(from.GetRow(0), to.GetRow(0), t));
        newMatrix.SetRow(1, Vector4.Lerp(from.GetRow(1), to.GetRow(1), t));
        newMatrix.SetRow(2, Vector4.Lerp(from.GetRow(2), to.GetRow(2), t));
        newMatrix.SetRow(3, Vector4.Lerp(from.GetRow(3), to.GetRow(3), t));
        return newMatrix;
    }
}