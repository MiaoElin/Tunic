using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class RoleAINodeTM {

    public RoleAINodeType type;
    public List<RoleAINodeSO> childrens;
    public RoleAIPreconditionGroupType preconditionGroupType;
    public RoleAIPreconditionSO[] preconditionSOs;
    public RoleAIAcitonSO acitonSO;
}