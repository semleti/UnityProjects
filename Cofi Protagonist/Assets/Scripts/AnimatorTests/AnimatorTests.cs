using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorTests : MonoBehaviour {

    [Serializable]
    public struct data{
        public int age;
        public float time;
        public GameObject obj;
    }
    private Animator animator;
    public data[] datas;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach( var clip in clips)
        {
            Debug.Log(clip);
            Animation anim = new Animation();
            foreach( AnimationState s in anim)
            {
                
            }
        }

        install(typeof(AnimatorTests),"hello1", typeof(AnimatorTests), "hello2");
        hello2();


    }

    public void hello1()
    {
        Debug.Log("hello1");
    }

    public void hello2()
    {
        Debug.Log("hello2");
    }


    public static void install(Type type1, String func1, Type type2, String func2)
    {
        Debug.Log("installing");
        MethodInfo methodToReplace = type1.GetMethod(func1, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
        MethodInfo methodToInject = type2.GetMethod(func2, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
        RuntimeHelpers.PrepareMethod(methodToReplace.MethodHandle);
        RuntimeHelpers.PrepareMethod(methodToInject.MethodHandle);

        unsafe
        {
            if (IntPtr.Size == 4)
            {
                int* inj = (int*)methodToInject.MethodHandle.Value.ToPointer() + 2;
                int* tar = (int*)methodToReplace.MethodHandle.Value.ToPointer() + 2;
#if DEBUG
                Debug.Log("\nVersion x86 Debug\n");

                byte* injInst = (byte*)*inj;
                byte* tarInst = (byte*)*tar;

                int* injSrc = (int*)(injInst + 1);
                int* tarSrc = (int*)(tarInst + 1);

                *tarSrc = (((int)injInst + 5) + *injSrc) - ((int)tarInst + 5);
#else
                    Debug.Log("\nVersion x86 Release\n");
                    *tar = *inj;
#endif
            }
            else
            {

                long* inj = (long*)methodToInject.MethodHandle.Value.ToPointer() + 1;
                long* tar = (long*)methodToReplace.MethodHandle.Value.ToPointer() + 1;
#if DEBUG
                Debug.Log("\nVersion x64 Debug\n");
                byte* injInst = (byte*)*inj;
                byte* tarInst = (byte*)*tar;


                int* injSrc = (int*)(injInst + 1);
                int* tarSrc = (int*)(tarInst + 1);

                *tarSrc = (((int)injInst + 5) + *injSrc) - ((int)tarInst + 5);
#else
                    Debug.Log("\nVersion x64 Release\n");
                    *tar = *inj;
#endif
            }
        }
        Debug.Log("installed");
    }


    public void injectionMethod(GameObject go, float time)
    {
        Debug.Log(time + " : " + go);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
