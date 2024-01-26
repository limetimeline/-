using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class NoteManagerBASS : MonoBehaviour
{

    public GameObject[] Effect; // 0 : Perfect, 1 : Good, 2 : Bad
    enum effectEnum {Perfect, Good, Bad};

    int bpm = 127;
    int index = 0;
    double currentTime = 0d;
    public float note_speed;

    bool isEffect = false;
    GameObject currentEffect = null;
    public GameObject effectPosition = null;
    int currentEffectNum = 999;

    [SerializeField] Transform[] tfNoteAppear = null;
    [SerializeField] GameObject goNote = null;

    public Transform Center;
    public RectTransform[] timingRect = null; // 0 : PerfectRect, 1 : GoodRect
    Vector2[] timingBoxs = null;

    public List<GameObject> noteList = new List<GameObject>();

    float time = 0;
    int timer = 0;
    string[] lines;
    string[] filePath = { "bass", "TONE", "BASS TONE 1", "BASS TONE 2", "BASS TONE 3" };
    string value;

    void Start()
    {
        Init();

/*        filePath[0] = "bass"; // .txt는 필요없다.
        filePath[1] = "bass 1";
        filePath[2] = "bass 2";
        filePath[3] = "bass 3";
        filePath[4] = "bass 4";*/

        Debug.Log(SceneTrans.bass);
        TextAsset asset = Resources.Load(filePath[SceneTrans.bass]) as TextAsset; // Resource아래 폴더가 더 있다면 /test/filename
        
        value = asset.text;

        lines = value.Split('\n');
    }

    // Update is called once per frame
    void Update()
    {
        if (BASSManager.gameStart && !PauseBASS.isPause)
        {
            StartCoroutine("DelayNote");

            if (BASSManager.currentLoud > -22.5f)//(Input.GetKeyDown(KeyCode.Space))// 
            {
                CheckingTiming();
            }
        }
    }

    IEnumerator DelayNote()
    {
        yield return new WaitForSeconds(0f);
        if (time <= note_speed)
        {
            time += Time.deltaTime;
        }
        else
        {
            time = 0;

            
            Debug.Log(timer);
            if (timer < lines.Length)
            {
                if (int.Parse(lines[timer]) / 1000 == 1)
                {
                    GameObject t_note = Instantiate(goNote, tfNoteAppear[0].transform.position, tfNoteAppear[0].transform.rotation);
                    t_note.transform.SetParent(this.transform);
                    noteList.Add(t_note);
                }
                else if (int.Parse(lines[timer]) / 100 == 1)
                {
                    GameObject t_note = Instantiate(goNote, tfNoteAppear[1].transform.position, tfNoteAppear[1].transform.rotation);
                    t_note.transform.SetParent(this.transform);
                    noteList.Add(t_note);
                }
                else if (int.Parse(lines[timer]) / 10 == 1)
                {
                    GameObject t_note = Instantiate(goNote, tfNoteAppear[2].transform.position, tfNoteAppear[2].transform.rotation);
                    t_note.transform.SetParent(this.transform);
                    noteList.Add(t_note);
                }
                else if (int.Parse(lines[timer]) / 1 == 1)
                {
                    GameObject t_note = Instantiate(goNote, tfNoteAppear[3].transform.position, tfNoteAppear[3].transform.rotation);
                    t_note.transform.SetParent(this.transform);
                    noteList.Add(t_note);
                    
                }
                timer++;
            }
            if(timer >= lines.Length)
            {
                timer = 0;
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
            if (other.tag == "Note")
            {
                if (noteList.Contains(other.gameObject))
                {
                    noteList.Remove(other.gameObject);
                    InitiateEffect((int)effectEnum.Bad);
            }
                Destroy(other.gameObject);
            }
    }

    void Init()
    {
        timingBoxs = new Vector2[timingRect.Length]; // Good, Perfect

        for (int i = 0; i < timingBoxs.Length; i++)
        {
            timingBoxs[i].Set(Center.localPosition.y - timingRect[i].rect.height / 2,
                Center.localPosition.y + timingRect[i].rect.height / 2);
        } // 판정 범위 최소, 최대

        Debug.Log($"0 : {timingBoxs[0]}, 1 : {timingBoxs[1]}");
    }

    void CheckingTiming()
    {
        for (int i = 0; i < noteList.Count; i++)
        {
            float t_notePosY = noteList[i].transform.localPosition.y;
            for (int y = 0; y < timingBoxs.Length; y++)
            {
                if (timingBoxs[y].x <= t_notePosY && t_notePosY <= timingBoxs[y].y)
                {
                    if (y == 0) // Perfect
                    {
                        noteList[i].GetComponent<NoteBASS>().HideNote();
                        InitiateEffect((int)effectEnum.Perfect);
                    }
                    else if (y == 1) // Good
                    {
                        noteList[i].GetComponent<NoteBASS>().HideNote();
                        InitiateEffect((int)effectEnum.Good);
                    }
                    noteList.RemoveAt(i);
                    Debug.Log("Hit" + y);
                    return;
                }
            }
        }
        Debug.Log("Miss");

    }

    void InitiateEffect(int num)
    {
        if (currentEffectNum != num)
        {
            if (currentEffect != null)
            {
                Destroy(currentEffect);
            }
            currentEffect = Instantiate(Effect[num], effectPosition.transform.position, effectPosition.transform.rotation);
            currentEffect.transform.SetParent(effectPosition.transform);
            currentEffectNum = num;
        }
        else
        {
            return;
        }
    }

}
