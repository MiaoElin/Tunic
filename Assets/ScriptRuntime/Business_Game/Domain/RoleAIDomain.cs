using System;
using UnityEngine;
using static RoleAINodeStatus;

public static class RoleAIDomain {

    public static void Tick(GameContext ctx, RoleEntity role, float dt) {
        var aiCom = role.aiCom;
        var root = aiCom.tree.root;
        RoleAINodeStatus status = Execute(ctx, role, root, dt);
        if (status == RoleAINodeStatus.Done) {
            root.Reset();
        }
    }

    public static RoleAINodeStatus Execute(GameContext ctx, RoleEntity role, RoleAINodeModel node, float dt) {
        var type = node.type;
        ref var status = ref node.status;

        if (type == RoleAINodeType.Action) {
            status = Action_Execute(ctx, role, node, dt);
        } else if (type == RoleAINodeType.SelectorSequence) {
            // 单次执行一个 按顺序找到第一个是可进入Running的节点（notEnter条件为true，或者是Running状态）
            status = Container_SelectorSequence_Execute(ctx, role, node, dt);
        } else if (type == RoleAINodeType.SelectorRandom) {
            // 单次执行一个 打乱顺序,找到第一个是可进入Running的节点（notEnter条件为true，或者是Running状态）
            status = Container_SelectorRandom_Execute(ctx, role, node, dt);
        } else if (type == RoleAINodeType.Sequence) {
            // 单次执行一个 按顺序找到第一个为!done状态的节点，Notenter也算（不管能不能执行，不能执行就不能进入下一个，返回done，要重新进入这个顺序容器，从头开始执行）
            status = Container_Sequence_Execute(ctx, role, node, dt);
        } else if (type == RoleAINodeType.ParallelAnd) {
            // 单次同时执行 要所有的都done了算结束
            status = Container_ParallelAnd_Execute(ctx, role, node, dt);
        } else if (type == RoleAINodeType.ParallelOr) {
            // 单次同时执行 有一个执行完了就算结束
            status = Container_ParallelOr_Execute(ctx, role, node, dt);
        }
        return status;
    }

    static bool ALlowExecute(GameContext ctx, RoleEntity role, RoleAINodeModel node, float dt) {
        if (node.status == Done) {
            return false;
        } else if (node.status == NotEnter) {
            if (!Check_Preconditions(ctx, role, node, dt)) {
                return false;
            } else {
                return true;
            }
        } else {
            return true;
        }
    }

    private static RoleAINodeStatus Action_Execute(GameContext ctx, RoleEntity role, RoleAINodeModel node, float dt) {
        ref var status = ref node.status;

        if (status == RoleAINodeStatus.NotEnter) {
            if (!Check_Preconditions(ctx, role, node, dt)) {
                status = RoleAINodeStatus.Done;
            } else {
                status = Action_Enter(role, node, dt);
            }

        } else if (status == RoleAINodeStatus.Running) {
            status = Action_Update(ctx, role, node, dt);
        }
        return status;
    }

    private static RoleAINodeStatus Container_SelectorSequence_Execute(GameContext ctx, RoleEntity role, RoleAINodeModel node, float dt) {
        ref var status = ref node.status;
        if (status == RoleAINodeStatus.NotEnter) {
            if (!Check_Preconditions(ctx, role, node, dt)) {
                status = Done;
            } else {
                status = Running;
            }
        } else {
            if (node.activeChild != null) {
                RoleAINodeStatus childStatus = Execute(ctx, role, node.activeChild, dt);
                if (childStatus == Done) {
                    status = Done;
                }
            } else {
                foreach (var child in node.childrens) {
                    bool allow = ALlowExecute(ctx, role, child, dt);
                    if (allow) {
                        RoleAINodeStatus childStatus = Execute(ctx, role, child, dt);
                        if (childStatus != Done) {
                            node.activeChild = child;
                            break;
                        }
                    }
                }
                if (node.activeChild == null) {
                    status = Done;
                }
            }
        }
        return status;
    }

    private static RoleAINodeStatus Container_SelectorRandom_Execute(GameContext ctx, RoleEntity role, RoleAINodeModel node, float dt) {
        ref var status = ref node.status;
        if (status == RoleAINodeStatus.NotEnter) {
            if (!Check_Preconditions(ctx, role, node, dt)) {
                status = Done;
            } else {
                status = Running;
                for (int i = 0; i < node.childrens.Count; i++) {
                    var index = UnityEngine.Random.Range(i, node.childrens.Count);
                    var temp = node.childrens[i];
                    node.childrens[i] = node.childrens[index];
                    node.childrens[index] = temp;
                }
            }
        } else if (status == RoleAINodeStatus.Running) {
            if (node.activeChild != null) {
                RoleAINodeStatus childStatus = Execute(ctx, role, node.activeChild, dt);
                if (childStatus == Done) {
                    status = Done;
                }
            } else {
                foreach (var child in node.childrens) {
                    bool allow = ALlowExecute(ctx, role, child, dt);
                    if (allow) {
                        RoleAINodeStatus childStatus = Execute(ctx, role, child, dt);
                        if (childStatus != Done) {
                            node.activeChild = child;
                            break;
                        }
                    }
                }
                if (node.activeChild == null) {
                    status = Done;
                }
            }
        }
        return status;
    }

    private static RoleAINodeStatus Container_Sequence_Execute(GameContext ctx, RoleEntity role, RoleAINodeModel node, float dt) {
        ref var stauts = ref node.status;
        if (stauts == NotEnter) {
            if (!Check_Preconditions(ctx, role, node, dt)) {
                stauts = Done;
            } else {
                stauts = Running;
            }
        } else if (stauts == Running) {
            int doneCount = 0;
            foreach (var child in node.childrens) {
                if (child.status != Done) {
                    _ = Execute(ctx, role, child, dt);
                    break;
                } else {
                    doneCount++;
                }
            }
            if (doneCount == node.childrens.Count) {
                stauts = Done;
            }
        }
        return stauts;
    }

    private static RoleAINodeStatus Container_ParallelAnd_Execute(GameContext ctx, RoleEntity role, RoleAINodeModel node, float dt) {
        ref var status = ref node.status;
        if (status == NotEnter) {
            if (!Check_Preconditions(ctx, role, node, dt)) {
                status = Done;
            } else {
                status = Running;
            }
        } else if (status == Running) {
            int doneCount = 0;
            foreach (var child in node.childrens) {
                RoleAINodeStatus childStatus = Execute(ctx, role, node, dt);
                if (childStatus == Done) {
                    doneCount++;
                }
            }
            if (doneCount == node.childrens.Count) {
                status = Done;
            }
        }
        return status;
    }

    private static RoleAINodeStatus Container_ParallelOr_Execute(GameContext ctx, RoleEntity role, RoleAINodeModel node, float dt) {
        ref var status = ref node.status;
        if (status == NotEnter) {
            if (!Check_Preconditions(ctx, role, node, dt)) {
                status = Done;
            } else {
                status = Running;
            }
        } else if (status == Running) {
            int doneCount = 0;
            foreach (var child in node.childrens) {
                RoleAINodeStatus childStatus = Execute(ctx, role, node, dt);
                if (childStatus == Done) {
                    doneCount++;
                }
            }
            if (doneCount > 0) {
                status = Done;
            }
        }
        return status;
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

        if (preconditionModel.isNeedTarget) {
            if (!hasTarget) {
                return false;
            }
        }

        if (preconditionModel.isNoTarget) {
            if (hasTarget) {
                return false;
            }
        }

        if (preconditionModel.isInAttackRange) {
            if (!hasTarget) {
                return false;
            }
            if (aiCom.entityType == EntityType.Role) {
                bool has = ctx.roleRepo.TryGet(aiCom.targetID, out var target);
                if (!has) {
                    return false;
                }
                Vector3 dir = target.Pos() - role.Pos();
                if (Vector3.SqrMagnitude(dir) > Mathf.Pow(role.attackRange, 2)) {
                    return false;
                }
            }
        }

        return true;
    }

    public static RoleAINodeStatus Action_Enter(RoleEntity role, RoleAINodeModel node, float dt) {
        var actionModel = node.actionModel;
        if (actionModel.isSearchTarget) {
            return RoleAINodeStatus.Running;
        }

        if (actionModel.isMoveToTarget) {
            return Running;
        }

        if (actionModel.isCastingSkill) {
            bool has = RoleDomain.HasUsableWeapon(role);
            if (has) {
                // 后面改成改变输入控制的方式
                RoleDomain.AI_SetCastingWeapon(role);
                role.fsm.EnterCasting();
                return Running;
            }
        }
        return Done;

    }

    private static RoleAINodeStatus Action_Update(GameContext ctx, RoleEntity role, RoleAINodeModel node, float dt) {
        var actionModel = node.actionModel;
        if (actionModel.isSearchTarget) {
            return Action_SearchUpdate(ctx, role, node, dt);
        }
        if (actionModel.isCastingSkill) {
            return Action_CastingUpdate(ctx, role, node, dt);
        }
        return RoleAINodeStatus.Done;
    }

    private static RoleAINodeStatus Action_SearchUpdate(GameContext ctx, RoleEntity role, RoleAINodeModel node, float dt) {
        var actionModel = node.actionModel;
        if (actionModel.isSearchPlayerOwner) {
            var owenr = ctx.GetOwner();
            Vector3 dir = owenr.Pos() - role.Pos();
            if (Vector3.SqrMagnitude(dir) <= Mathf.Pow(role.searchRange, 2)) {
                role.aiCom.entityType = EntityType.Role;
                role.aiCom.targetID = owenr.id;
            } else {
                role.aiCom.entityType = EntityType.None;
                role.aiCom.targetID = 0;
            }
        } else {

        }
        return RoleAINodeStatus.Done;
    }

    private static RoleAINodeStatus Action_CastingUpdate(GameContext ctx, RoleEntity role, RoleAINodeModel node, float dt) {
        var weapon = role.GetCastingWeapon();
        if (weapon == null) {
            return Done;
        } else {
            return Running;
        }
    }
}