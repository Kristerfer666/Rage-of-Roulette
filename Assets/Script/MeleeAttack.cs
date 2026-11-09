using System.Collections;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public GameObject appearObject;
    private SimpleSpriteAnimation attackAnim;
    private bool isVisible = false;

    void Start()
    {
        attackAnim = appearObject.GetComponent<SimpleSpriteAnimation>();
        appearObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isVisible)
        {
            StartCoroutine(AppearRoutine());
        }
    }

    IEnumerator AppearRoutine()
    {
        isVisible = true;
        appearObject.SetActive(true);

        float duration = attackAnim.frames.Length / attackAnim.frameRate;
        yield return new WaitForSeconds(duration);

        appearObject.SetActive(false);
        isVisible = false;
    }

    
}