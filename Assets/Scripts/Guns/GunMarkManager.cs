using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMarkManager : MonoBehaviour
{
    #region Singleton
    static GunMarkManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }
    public static GunMarkManager Instance { get { return instance; } }
    #endregion

    [SerializeField] Transform parent;
    [SerializeField] int maxNumberOfMarks = 30;
    [SerializeField] List<GameObject> marks;
    [SerializeField] GameObject gunMark;
    [SerializeField] int lastMark = 0;

    void Start()
    {
        marks = new List<GameObject>(maxNumberOfMarks);
        for(int i = 0; i < maxNumberOfMarks; i++)
            marks.Add(Instantiate(gunMark, new Vector3(0,-50,0), Quaternion.identity, parent));

    }

    public GameObject GetMark()
    {
        GameObject mark = marks[lastMark];
        lastMark++;
        if(lastMark == maxNumberOfMarks)
            lastMark = 0;

        return mark;
    }
}
