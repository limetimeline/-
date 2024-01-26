using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class NoteManager : MonoBehaviour
{

    public GameObject[] Effect; // 0 : Perfect, 1 : Good, 2 : Bad
    enum effectEnum {Perfect, Good, Bad};

    int bpm = 127;
    int index = 0;
    double currentTime = 0d;
    float note_speed;

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
    string filePath;
    string value;

    void Start()
    {
        Init();

        if (Application.platform == RuntimePlatform.Android)
        {
            /* Resource 아래 폴더 - Resource는 빌드해도 남아 있다. 나머지는 다 압축된다. */
            filePath = "gumi"; // .txt는 안쓴다고 한다.
            TextAsset asset = Resources.Load(filePath) as TextAsset; // Resource아래 폴더가 더 있다면 /test/filename
            value = asset.text;
            note_speed = 0.133333f;
        }
        else
        {
            /*            filePath = Path.Combine(Application.streamingAssetsPath, "gumi.txt"); 
                        value = ReadTxt(filePath);*/
            filePath = "gumi";
            TextAsset asset = Resources.Load(filePath) as TextAsset; // Resource아래 폴더가 더 있다면 /test/filename
            value = asset.text;
            note_speed = 0.13374f;
        }
        lines = value.Split('\n');



    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameStart && !Pause.isPause)
        {
            if (!GameManager.gameEnd)
            {
                if (GameManager.musicStart)
                {
                    StartCoroutine("DelayNote");

                
                }


/*                currentTime += Time.deltaTime;
                if (currentTime >= 60d / bpm)
                {
                    //GameObject t_note = Instantiate(goNote, tfNoteAppear.position, Quaternion.identity);
                    index = Random.Range(0, 4);
                    GameObject t_note = Instantiate(goNote, tfNoteAppear[index].transform.position, tfNoteAppear[index].transform.rotation);
                    t_note.transform.SetParent(this.transform);
                    noteList.Add(t_note);
                    currentTime -= 60d / bpm;
                }*/

                if (GameManager.currentLoud > -28.0f)//(Input.GetKeyDown(KeyCode.Space))// 
                {
                    CheckingTiming();
                }

            }
        }
        
    }

    IEnumerator DelayNote()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            yield return new WaitForSeconds(7.3f);
        }
        else
        {
            yield return new WaitForSeconds(7.8f);
        }
        if (time <= note_speed)
        {
            time += Time.deltaTime;
        }
        else
        {
            time = 0;

            timer++;
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
                    GameManager.score[2]++;
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
                        GameManager.score[0]++;
                        noteList[i].GetComponent<Note>().HideNote();
                        InitiateEffect((int)effectEnum.Perfect);
                    }
                    else if (y == 1) // Good
                    {
                        GameManager.score[1]++;
                        noteList[i].GetComponent<Note>().HideNote();
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

    string ReadTxt(string filePath)
    {
        FileInfo fileInfo = new FileInfo(filePath);
        string value = "";

        if (fileInfo.Exists)
        {
            StreamReader reader = new StreamReader(filePath);
            value = reader.ReadToEnd();
            reader.Close();
        }

        else
            value = "파일이 없습니다.";

        return value;
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
