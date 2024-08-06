using System;
using UnityEngine;

public static class RoleAIDomain {

    public static void Tick(RoleEntity role, float dt) {
        var aiCom = role.aiCom;
        var root = aiCom.tree.root;
        RoleAINodeStatus status = Execute(role, root, dt);
        if (status == RoleAINodeStatus.Done) {
            root.Reset();
        }
    }

    public static RoleAINodeStatus Execute(GameContext ctx, RoleEntity role, RoleAINodeModel node, float dt) {
        var type = node.type;

        if (type == RoleAINodeType.Action) {
            node.status = Action_Execute(ctx, role, node, dt);
        }
        return node.status;
    }

    private static RoleAINodeStatus Action_Execute(GameContext ctx, RoleEntity role, RoleAINodeModel node, float dt) {
        ref var status = ref node.status;

        if (status == RoleAINodeStatus.NotEnter) {
            if (!Check_Preconditions(ctx, role, node, dt)) {
                status = RoleAINodeStatus.Done;
            } else {
                status = RoleAINodeStatus.Running;
            }

        } else if (status == RoleAINodeStatus.Running) {
            status = Action_Update(node, dt);
        }
        return status;
    }

    private static RoleAINodeStatus Action_Update(RoleAINodeModel node, float dt) {
        throw new NotImplementedException();
    }

    private static bool Check_Preconditions(GameContext ctx, RoleEntity role, RoleAINodeModel node, float dt) {
        if (node.preconditionModels == null) {
            return true;
        }

        if (node.preconditionGroupType == RoleAIPreconditionGroupType.And) {
            foreach (var precondition in node.preconditionModels) {
                bool isTrue = Check_Precondition(ctx, role, precondition, dt);
                if (!isTrue) {
                    return false;
                }
            }
            return true;
        } else if (node.preconditionGroupType == RoleAIPreconditionGroupType.Or) {
            foreach (var precondition in node.preconditionModels) {
                bool isTrue = Check_Precondition(ctx, role, precondition, dt);
                if (isTrue) {
                    return true;
                }
            }
            return false;
        } else {
            Debug.LogError("");
            return false;
        }

    }

    public static bool Check_Precondition(GameContext ctx, RoleEntity role, RoleAIPreconditionModel preconditionModel, float dt) {
        if (preconditionModel == null) {
            return true;
        }
        var aiCom = role.aiCom;
        bool hasTarget = aiCom.entityType != EntityType.None;

        if (preconditionModel.hasTarget) {
            if (!hasTarget) {
                return false;
            }
        }

        if (preconditionModel.isNoTarget) {
            if (hasTarget) {
                return false;
            }
        }

        if (preconditionModel.isNearTarget) {
            if (!hasTarget) {
                return false;
            }
            if (aiCom.entityType == EntityType.Role) {
                bool has = ctx.roleRepo.TryGet(aiCom.targetID, out var target);
                if (!has) {
                    return false;
                }
                Vector3 dir = target.Pos() - role.Pos();
                if (Vector3.SqrMagnitude(dir) > Mathf.Pow(preconditionModel.nearTarget_Distance, 2)) {
                    return false;
                }
            }
        }
        return true;
    }
}