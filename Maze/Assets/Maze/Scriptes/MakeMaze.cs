using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeMaze : MonoBehaviour
{
    public GameObject[] cube;
    List<int> Maze = new List<int>();
    List<int> MazeObject = new List<int>();
    List<float> MazeDeg= new List<float>();
    public GameObject Game, pre,gold;

    // Start is called before the first frame update
    void Start()
    {
        for (int j = 0; j < 43; j++)
        {
            for (int i = 0; i < 43; i++)
            { 
                int num;
                num = 0;
                if (i == 0 || i == 42 || j == 0 || j == 42) num = 1;
                if (i >= 2 && i <= 40 && j >= 2 && j <= 40)
                {
                    if (i % 2 == 0 && j % 2 == 0) num = 1;
                }
                Maze.Add(num);
                MazeObject.Add(6);
                MazeDeg.Add(0);
            }
        }
        for (int j = 2; j <= 40; j+=2)
        {
            for (int i = 2; i <= 40; i+=2)
            {
                int a, dx=0, dy=0;
                do
                {
                    if (i == 2) a = Random.Range(0, 4);
                    else a = Random.Range(0, 3);
                    switch (a)
                    {
                        case 0:
                            dx = 0;
                            dy = -1;
                            break;
                        case 1:
                            dx = 1;
                            dy = 0;
                            break;
                        case 2:
                            dx = 0;
                            dy = 1;
                            break;
                        case 3:
                            dx = -1;
                            dy = 0;
                            break;
                    }
                } while (Maze[(j+dy) * 43 + (i+dx)] == 1);
                Maze[(j + dy) * 43 + (i + dx)] = 1;
            }
        }
        int[] table = new int[4] { 1, 2, 4, 8 };
        for (int j = 0; j < 43; j++)
        {
            for (int i = 0; i < 43; i++)
            {
                if (Maze[j * 43 + i] == 1)
                {
                    int dx = 0, dy = -1;
                    int data = 0;
                    for (int k = 0; k < 4; k++)
                    {
                        int temp;
                        //Debug.Log("dx:" + dx + ",dy:" + dy);
                        if ((j + dy) >= 0 && (i + dx) >= 0 && (j + dy) <= 42 && (i + dx) <= 42)
                        {

                            if (Maze[(j + dy) * 43 + (i + dx)] == 1)
                            {
                                data += table[k];
                            }
                        }
                        else
                        {
                            data += table[k];
                        }
                        temp = -dy;
                        dy = dx;
                        dx = temp;
                    }
                    if (data == 15)
                    {
                        MazeObject[j * 43 + i] = 0;
                    }
                    if (data == 7 || data == 11 || data == 13 || data == 14)
                    {
                        MazeObject[j * 43 + i] = 1;
                        if (data == 11) MazeDeg[j * 43 + i] = 90;
                        if (data == 13) MazeDeg[j * 43 + i] = 180;
                        if (data == 14) MazeDeg[j * 43 + i] = 270;
                    }
                    if (data == 3 || data == 6 || data == 9 || data == 12)
                    {
                        MazeObject[j * 43 + i] = 2;
                        if (data == 6) MazeDeg[j * 43 + i] = 270;
                        if (data == 9) MazeDeg[j * 43 + i] = 90;
                        if (data == 12) MazeDeg[j * 43 + i] = 180;

                    }
                    if (data == 5 || data == 10)
                    {
                        MazeObject[j * 43 + i] = 3;
                        if (data == 10) MazeDeg[j * 43 + i] = 90;
                    }
                    if (data == 1 || data == 2 || data == 4 || data == 8)
                    {
                        MazeObject[j * 43 + i] = 4;
                        if (data == 1) MazeDeg[j * 43 + i] = 0;
                        if (data == 2) MazeDeg[j * 43 + i] = 270;
                        if (data == 4) MazeDeg[j * 43 + i] = 180;
                        if (data == 8) MazeDeg[j * 43 + i] = 90;

                    }

                    if (data == 0)
                    {
                        MazeObject[j * 43 + i] = 5;
                    }

                }
            }
        }

        for (int j = 0; j < 43; j++)
        {
            for (int i = 0; i < 43; i++)
            {
                int num = MazeObject[j * 43 + i];
                GameObject obj = Instantiate(cube[num]);
                Vector3 angle=obj.transform.localEulerAngles;
                angle.y = MazeDeg[j * 43 + i];
                obj.transform.localEulerAngles = angle;
                obj.transform.position = new Vector3((i - 22) * 6, obj.transform.position.y, (j - 22) * 6);
                obj.transform.SetParent(pre.transform);
            }
        }
        
        Component[] meshFilters = pre.GetComponentsInChildren<MeshFilter>();
        Dictionary<Material, List<CombineInstance>> combineMeshInstanceDictionary = new Dictionary<Material, List<CombineInstance>>();

        foreach (MeshFilter mesh in meshFilters)
        {
            var mat = mesh.GetComponent<Renderer>().sharedMaterial;
            if (mat == null) continue;
            if (!combineMeshInstanceDictionary.ContainsKey(mat))
            {
                combineMeshInstanceDictionary.Add(mat, new List<CombineInstance>());
            }
            var instance = combineMeshInstanceDictionary[mat];
            CombineInstance conbines = new CombineInstance();
            conbines.mesh = mesh.sharedMesh;
            conbines.transform = mesh.transform.localToWorldMatrix;
            Destroy(mesh.gameObject.GetComponent<MeshFilter>());
            Destroy(mesh.gameObject.GetComponent<MeshRenderer>());
            mesh.gameObject.isStatic = true;
            instance.Add(conbines);
        }
        pre.isStatic = true;

        Game.isStatic = true;
        foreach (var dic in combineMeshInstanceDictionary)
        {
            if (dic.Key.name == "mat1") continue;
            var newObject = new GameObject(dic.Key.name);
            newObject.isStatic = true;
            var meshrenderer = newObject.AddComponent<MeshRenderer>();
            var meshfilter = newObject.AddComponent<MeshFilter>();
            meshrenderer.material = dic.Key;
            var mesh = new Mesh();
            mesh.CombineMeshes(dic.Value.ToArray());

            meshfilter.sharedMesh = mesh;
            newObject.transform.parent = Game.transform;
            if (newObject.name != "woodPlanks_bare03_bumped")
            {
                newObject.layer = 9;
            }
        }        
        //マテリアルを再設定
        Game.SetActive(true);
        for (int i = 0; i < 20; i++)
        {
            int x = 0, y = 0;
            do
            {
                x = Random.Range(0, 43);
                y = Random.Range(0, 43);
            } while (Maze[y * 43 + x] != 0);
            GameObject obj = Instantiate(gold);
            obj.transform.position = new Vector3((x - 22) * 6, obj.transform.position.y, (y - 22) * 6);
            Maze[y * 43 + x] = 2;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
