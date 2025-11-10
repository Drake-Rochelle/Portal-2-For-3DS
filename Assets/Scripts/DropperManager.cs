using System;
using System.Collections.Generic;
using UnityEngine;

public class DropperManager : MonoBehaviour 
{
	[SerializeField] private GameObject[] props;
	[SerializeField] private float[] probs;
	[SerializeField] private ButtonManager button;
    [SerializeField] private Transform[] propParents;
    [SerializeField] private Vector3 spawnOffset;
	private bool clear;
    private void Start()
    {
        int[] ints = Reduce(probs);
        List<GameObject> gos = new List<GameObject>();
        List<Transform> parents = new List<Transform>();
        for (int i = 0; i < props.Length; i++)
        {
            for (int j = 0; j < ints[i]; j++)
            {
                gos.Add(props[i]);
                parents.Add(propParents[i]);
            }
        }
        props = gos.ToArray();
        propParents = parents.ToArray();
    }
	void Update () 
	{
		if (clear)
		{
			clear = false;
			button.isPressed = false;
		}
		if (button.isPressed)
		{
			clear = true;
            int index = UnityEngine.Random.Range(0, props.Length);
            GameObject go = Instantiate(props[index], transform.position + spawnOffset, Quaternion.identity);
            go.transform.parent = propParents[index];
		}
	}
    private static int GCD(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
    private static int GCD(List<int> values)
    {
        int result = values[0];
        for (int i = 1; i < values.Count; i++)
            result = GCD(result, values[i]);
        return result;
    }
    public static int[] Reduce(float[] floatIns)
    {
        int mul = 1;
        bool allIntegers = false;
        while (!allIntegers)
        {
            allIntegers = true;
            mul *= 10;
            foreach (var f in floatIns)
            {
                if (Math.Abs(f * mul - Math.Round(f * mul)) > 1e-6)
                {
                    allIntegers = false;
                    break;
                }
            }
        }
        var scaled = new List<int>();
        foreach (var f in floatIns)
            scaled.Add((int)Math.Round(f * mul));
        int gcf = GCD(scaled);
        for (int i = 0; i < scaled.Count; i++)
            scaled[i] /= gcf;
        return scaled.ToArray();
    }
}
