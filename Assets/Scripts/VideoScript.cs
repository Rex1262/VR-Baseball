using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoScript : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    [SerializeField]
    private VideoClip[] videoClips;
public static Transform CurentTransform;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.clip = videoClips[0];
        videoPlayer.Play();
        CurentTransform = gameObject.transform;
    }

    public void SetVideoClips()
    {
        switch (MirroringScript.mode)
        {
            case 0:
                videoPlayer.clip = videoClips[0];
                break;
            case 1:
                videoPlayer.clip = videoClips[1];
                break;
            case 2:
                videoPlayer.clip = videoClips[2];
                break;
            case 3:
                videoPlayer.clip = videoClips[3];
                break;
            default:
                break;
        }
    }

    public  static void ChangeScale(Transform transform)
    {
        transform.localScale = new Vector3(transform.localScale.x * -1,transform.localScale.y,transform.localScale.z);
    }
}
