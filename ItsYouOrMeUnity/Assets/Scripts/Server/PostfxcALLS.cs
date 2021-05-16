using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostfxcALLS : MonoBehaviour
{
    public Volume post;
    float v = 4.3f;
    float moveTime, start;
    [SerializeField] float depth = 4.3f;
    [SerializeField] float speed;
    DepthOfField dof;
    bool move;

    private void Start()
    {
        post.profile.TryGet(out dof);
    }

    public void ToScene()
    {
        start = 4.3f;
        v = 15.83f;
        move = true;
    }
    public void StopPost()
    {
        start = 15.83f;
        dof.focusDistance.value = 15.83f;
        depth = 15.83f;
        v = 15.83f;
        move = false;
        enabled = false;
    }
    public void QuickScene()
    {

        post.profile.TryGet(out dof);
        dof.focusDistance.value = 15.83f;
        start = 15.83f;
        depth = 15.83f;
        v = 15.83f;
        move = true;
        enabled = false;
    }
    public void MoveToMiniGame()
    {
        v = 0.1f;
    }
    void Update()
    {
        if(move)
        {
            moveTime += Time.deltaTime * speed;
            depth = Mathf.Lerp(start, v, moveTime);
            dof.focusDistance.value = depth;
            if(depth >= v -0.2f)
            {
                move = false;
            }
        }

    }
}
