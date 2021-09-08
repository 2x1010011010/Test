using UnityEngine;
using UnityEngine.EventSystems;

public class CollisionHandler : MonoBehaviour, IPointerClickHandler
{
    private MeshDestroy _destroyer;

    public void OnPointerClick(PointerEventData eventData)
    {
        _destroyer = GetComponent<MeshDestroy>();
        _destroyer.DestroyMesh();
    }
}
