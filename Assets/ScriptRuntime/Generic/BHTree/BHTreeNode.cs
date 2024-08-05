using UnityEngine;
using System.Collections.Generic;
using System;
public class BHTreeNode {
    public BHTreeNodeStatus status;
    public BHTreeNodeType type;
    public List<BHTreeNode> childrens;

    public BHTreeNode activeChild;

    public Func<bool> PreconditionHandle;

    public Func<float, BHTreeNodeStatus> ActNotEnterHandle;
    public Func<float, BHTreeNodeStatus> ActEnterHandle;
    public Func<float, BHTreeNodeStatus> ActRunningHandle;

    public void InitAction() {
        // 不设置就默认是枚举的第一个，这里可以不用赋值
        type = BHTreeNodeType.Action;
        status = BHTreeNodeStatus.NotEnter;
    }

    public void InitContainer(BHTreeNodeType type) {
        if (type == BHTreeNodeType.Action) {
            throw new Exception("ActionNode can not be Container");
        }
        this.type = type;
        childrens = new List<BHTreeNode>();
        status = BHTreeNodeStatus.NotEnter;
    }

    public bool AllEnter() {
        if (status == BHTreeNodeStatus.Done) {
            return false;
        } else if (status == BHTreeNodeStatus.NotEnter) {
            if (PreconditionHandle == null || PreconditionHandle.Invoke()) {
                return true;
            } else {
                return false;
            }
        } else {
            // running
            return true;
        }
    }

    public void Reset() {
        status = BHTreeNodeStatus.NotEnter;
        if (type != BHTreeNodeType.Action) {
            foreach (var child in childrens) {
                child.Reset();
            }
        }
        activeChild = null;
    }

    public BHTreeNodeStatus Execute(float dt) {
        if (type == BHTreeNodeType.SelectorSequence) {
            // 单次执行一个 按顺序找到第一个是可进入Running的节点（notEnter条件为true，或者是Running状态）
            status = Container_SelectorSequence_Execute(dt);
        } else if (type == BHTreeNodeType.SelectorRandom) {
            // 单次执行一个 打乱顺序,找到第一个是可进入Running的节点（notEnter条件为true，或者是Running状态）
            status = Container_SelectorRandom_Execute(dt);
        } else if (type == BHTreeNodeType.Sequence) {
            // 单次执行一个 按顺序找到第一个为!done状态的节点，Notenter也算（不管能不能执行，不能执行就不能进入下一个，返回done，要重新进入这个顺序容器，从头开始执行）
            status = Container_Sequence_Execute(dt);
        } else if (type == BHTreeNodeType.ParallelAnd) {
            // 单次同时执行 要所有的都done了算结束
            status = Container_ParallelAnd_Execute(dt);
        } else if (type == BHTreeNodeType.ParallelOr) {
            // 单次同时执行 有一个执行完了就算结束
            status = Container_ParallelOr_Execute(dt);
        } else if (type == BHTreeNodeType.Action) {
            status = Action_Execute(dt);
        }
        return status;
    }

    #region Container
    private BHTreeNodeStatus Container_SelectorSequence_Execute(float dt) {
        if (status == BHTreeNodeStatus.NotEnter) {
            if (PreconditionHandle == null || PreconditionHandle.Invoke()) {
                status = BHTreeNodeStatus.Running;
            } else {
                status = BHTreeNodeStatus.Done;
            }
        } else if (status == BHTreeNodeStatus.Running) {
            // 执行一个
            // 有执行中的子节点，执行完这个就返回结果
            if (activeChild != null) {
                BHTreeNodeStatus childStatus = activeChild.Execute(dt);
                if (childStatus == BHTreeNodeStatus.Done) {
                    status = BHTreeNodeStatus.Done;
                }
            } else {
                // 没有执行中的子节点
                // 找到第一个非Done状态的子节点 设为activeChild
                for (int i = 0; i < childrens.Count; i++) {
                    BHTreeNode child = childrens[i];
                    BHTreeNodeStatus childStatus = child.Execute(dt);
                    if (childStatus != BHTreeNodeStatus.Done) {
                        activeChild = child;
                        break;
                    }
                }
                // 如果没找到，说明都Done了；返回done；
                if (activeChild == null) {
                    status = BHTreeNodeStatus.Done;
                }
            }

        } else {
            // 已经done了；啥也不做
        }
        return status;
    }

    private BHTreeNodeStatus Container_SelectorRandom_Execute(float dt) {
        if (status == BHTreeNodeStatus.NotEnter) {
            if (PreconditionHandle == null || PreconditionHandle.Invoke()) {
                status = BHTreeNodeStatus.Running;
                for (int i = 0; i < childrens.Count; i++) {
                    int j = UnityEngine.Random.Range(i, childrens.Count);
                    BHTreeNode temp = childrens[i];
                    childrens[i] = childrens[j];
                    childrens[j] = temp;
                }
            } else {
                status = BHTreeNodeStatus.Done;
            }
        } else if (status == BHTreeNodeStatus.Running) {
            if (activeChild != null) {
                BHTreeNodeStatus childStatus = activeChild.Execute(dt);
                if (childStatus == BHTreeNodeStatus.Done) {
                    status = BHTreeNodeStatus.Done;
                }
            } else {
                foreach (var child in childrens) {
                    bool allow = child.AllEnter();
                    if (allow) {
                        BHTreeNodeStatus childStatus = activeChild.Execute(dt);
                        if (childStatus == BHTreeNodeStatus.Done) {
                            status = BHTreeNodeStatus.Done;
                        } else {
                            activeChild = child;
                        }
                        break;
                    }
                }
                if (activeChild == null) {
                    status = BHTreeNodeStatus.Done;
                }
            }
        } else {

        }
        return status;
    }

    private BHTreeNodeStatus Container_Sequence_Execute(float dt) {
        if (status == BHTreeNodeStatus.NotEnter) {
            if (PreconditionHandle == null || PreconditionHandle.Invoke()) {
                status = BHTreeNodeStatus.Running;
            } else {
                status = BHTreeNodeStatus.Done;
            }
        } else if (status == BHTreeNodeStatus.Running) {
            int doneCount = 0;
            // 按顺序，遇到的第一个非Done的子节点，执行它
            for (int i = 0; i < childrens.Count; i++) {
                BHTreeNode child = childrens[i];
                if (child.status == BHTreeNodeStatus.NotEnter) {
                    _ = child.Execute(dt);
                    break;
                } else if (child.status == BHTreeNodeStatus.Running) {
                    _ = child.Execute(dt);
                    break;
                } else {
                    // 计算done的数量
                    doneCount++;
                }
            }
            // 所有的子节点都已经done了
            if (doneCount >= childrens.Count) {
                status = BHTreeNodeStatus.Done;
            }

        } else {
            // 已经done了；啥也不做
        }
        return status;
    }

    private BHTreeNodeStatus Container_ParallelAnd_Execute(float dt) {
        if (status == BHTreeNodeStatus.NotEnter) {
            if (PreconditionHandle == null || PreconditionHandle.Invoke()) {
                status = BHTreeNodeStatus.Running;
            } else {
                status = BHTreeNodeStatus.Done;
            }
        } else if (status == BHTreeNodeStatus.Running) {

            int doneCount = 0;
            foreach (var child in childrens) {
                if (child.status == BHTreeNodeStatus.Done) {
                    doneCount++;
                } else {
                    BHTreeNodeStatus childStatus = child.Execute(dt);
                    if (childStatus == BHTreeNodeStatus.Done) {
                        doneCount++;
                    }
                }
            }
            if (doneCount >= childrens.Count) {
                status = BHTreeNodeStatus.Done;
            }
        }
        return status;
    }

    private BHTreeNodeStatus Container_ParallelOr_Execute(float dt) {
        if (status == BHTreeNodeStatus.NotEnter) {
            if (PreconditionHandle == null || PreconditionHandle.Invoke()) {
                status = BHTreeNodeStatus.Running;
            } else {
                if (ActNotEnterHandle != null) {
                    ActNotEnterHandle.Invoke(dt);
                }
                status = BHTreeNodeStatus.Done;
            }
        } else if (status == BHTreeNodeStatus.Running) {
            bool hasDone = false;
            foreach (var child in childrens) {
                BHTreeNodeStatus childStatus = child.Execute(dt);
                // if (childStatus == BHTreeNodeStatus.Done) {
                //     status = BHTreeNodeStatus.Done;
                //     break; 错误。这样执行一个done 其他就不执行了，这个要都执行，然后可以有多个done，至少有一个done就算成功
                // }
                if (childStatus == BHTreeNodeStatus.Done) {
                    hasDone = true;
                }
            }
            if (hasDone) {
                status = BHTreeNodeStatus.Done;
            }
        }
        return status;
    }
    #endregion

    #region Action
    private BHTreeNodeStatus Action_Execute(float dt) {
        if (status == BHTreeNodeStatus.NotEnter) {
            if (PreconditionHandle == null || PreconditionHandle.Invoke()) {
                status = BHTreeNodeStatus.Running;
                if (ActEnterHandle != null) {
                    status = ActEnterHandle.Invoke(dt);
                }
            } else {
                status = BHTreeNodeStatus.Done;
                if (ActNotEnterHandle != null) {
                    status = ActNotEnterHandle.Invoke(dt);
                }
            }
        } else if (status == BHTreeNodeStatus.Running) {
            if (ActRunningHandle == null) {
                Debug.Log("Action Was Not Setted");
            } else {
                status = ActRunningHandle.Invoke(dt);
            }
        } else {

        }
        return status;
    }
    #endregion
}