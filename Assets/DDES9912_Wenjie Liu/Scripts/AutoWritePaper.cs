using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutoWritePaper : MonoBehaviour
{
    public List<string> allChar = new List<string>();
    public List<ClickPoint> clickPoints = new List<ClickPoint>();
    public ClickCapPoint clickCap;

    public TextMeshPro aimText;
    public TypeWriter typeWriter;
    public Animator npcAni;
    public float idleTime, writeSpeed;

    public void Start()
    {
        StartCoroutine(LoopWriteText());
    }


    private IEnumerator LoopWriteText()
    {
        string info = aimText.text;
        Debug.Log(info);
        WaitForSeconds littleWait = new WaitForSeconds(writeSpeed);
        npcAni.SetBool("work", false);
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(idleTime + 1, idleTime + 3));
            typeWriter.InPaper();
            yield return new WaitForSeconds(2);
            npcAni.SetBool("work", true);

            foreach (var item in info)
            {
                string a = item.ToString();
                if (a.Equals("\n"))
                {
                    typeWriter.RunNewLine();
                    yield return new WaitForSeconds(1);
                    continue;
                }

                if (!allChar.Contains(a))
                {
                    a = a.ToLower();
                    clickCap.ClickDown();
                    yield return littleWait;
                    clickCap.ClickUp();
                    yield return littleWait;
                }
                if(allChar.Contains(a))
                {
                    clickPoints[allChar.IndexOf(a)].ClickDown();
                    yield return littleWait;
                    clickPoints[allChar.IndexOf(a)].ClickUp();
                    yield return littleWait;
                }
            }
            typeWriter.OutPaper();
            npcAni.SetBool("work",false);
        }
    }

}
