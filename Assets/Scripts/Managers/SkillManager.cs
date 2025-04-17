using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    #region Skills
    public Skill_Dash skill_dash;
    public Skill_Clone skill_clone;
    public Skill_ThrowSword skill_throwSword;
    #endregion
    private void Awake()
    {
        Debug.Log("LIFECYCLE::SkillManager awake");
        if (instance == null)
            instance = this;
        else
            Destroy(instance);

        #region GetComponent
        skill_dash = GetComponent<Skill_Dash>();
        skill_clone = GetComponent<Skill_Clone>();
        skill_throwSword = GetComponent<Skill_ThrowSword>();
        #endregion
    }
    void Start()
    {
        Debug.Log("LIFECYCLE::SkillManager start");
    }

    void Update()
    {
        
    }
}
