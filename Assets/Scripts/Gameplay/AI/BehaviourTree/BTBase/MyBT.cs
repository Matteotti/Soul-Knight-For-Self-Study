using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace MyBT
{
    /// <summary>
    /// BT node is the base of any nodes in BT framework.
    /// </summary>
    public abstract class BTNode
    {
        public string name;
        public BTNode parent;
        protected List<BTNode> _children;

        public List<BTNode> Children
        {
            get { return _children; }
        }

        public BTPrecondition precondition;
        public MyDatabase database;
        public float interval = 0;
        private float _lastTimeEvaluated = 0; //持续时间评估
        public bool activeself;

        public BTNode() : this(null)
        {
        }

        public BTNode(BTPrecondition precondition)
        {
            this.precondition = precondition;
        }

        public virtual void Activate(MyDatabase database)
        {
            if (activeself) return;
            this.database = database;
            if (precondition != null)
            {
                precondition.Activate(database);
            }

            if (_children != null)
            {
                foreach (BTNode child in _children)
                {
                    child.Activate(database);
                }
            }

            activeself = true;
        }

        public bool Evaluate()
        {
            bool coolDownOK = CheckTimer();
            return activeself && coolDownOK && (precondition == null || precondition.Check()) && DoEvaluate();
        }

        protected virtual bool DoEvaluate()
        {
            return true;
        }

        public virtual BTResult Tick()
        {
            return BTResult.Ended;
        }

        public virtual void Clear()
        {

        }

        public virtual void AddChild(BTNode aNode)
        {
            if (_children == null)
            {
                _children = new List<BTNode>();
            }

            if (aNode != null)
            {
                _children.Add(aNode);
            }
        }

        public virtual void RemoveChild(BTNode aNode)
        {
            if (_children != null && aNode != null)
            {
                _children.Remove(aNode);
            }
        }

        private bool CheckTimer()
        {
            if (Time.time - _lastTimeEvaluated > interval)
            {
                _lastTimeEvaluated = Time.time;
                return true;
            }

            return false;
        }

        public enum BTResult
        {
            Ended = 1,
            Running = 2,
        }
    }
    public abstract class BTPrecondition : BTNode
    {
        public BTPrecondition() : base(null) { }
        public abstract bool Check();
        public override BTResult Tick()
        {
            bool success = Check();
            if (success)
            {
                return BTResult.Ended;
            }
            else
            {
                return BTResult.Running;
            }
        }
    }
    /// <summary>
    /// A pre condition that uses database.
    /// </summary>
    public abstract class BTPreconditionUseDB : BTPrecondition
    {
        protected string _dataToCheck;
        protected int _dataIdToCheck;
        public BTPreconditionUseDB(string dataToCheck)
        {
            this._dataToCheck = dataToCheck;
        }
        public override void Activate(MyDatabase database)
        {
            base.Activate(database);
            _dataIdToCheck = database.GetDataId(_dataToCheck);
        }
    }
    /// <summary>
    /// Used to check if the float data in the database is less than / equal to / greater than the data passed in through constructor.
    /// </summary>
    public class BTPreconditionFloat : BTPreconditionUseDB
    {
        public float rhs;
        private FloatFunction func;
        public BTPreconditionFloat(string dataToCheck, FloatFunction func, float rhs) : base(dataToCheck)
        {
            this.func = func;
            this.rhs = rhs;
        }
        public override bool Check()
        {
            float lhs = database.GetData<float>(_dataIdToCheck);
            switch (func)
            {
                case FloatFunction.LessThan:
                    return lhs < rhs;
                case FloatFunction.GreaterThan:
                    return lhs > rhs;
                case FloatFunction.LessOrEqual:
                    return lhs <= rhs;
                case FloatFunction.GreaterOrEqual:
                    return lhs >= rhs;
                case FloatFunction.EqualTo:
                    return lhs == rhs;
            }
            return false;
        }

        public enum FloatFunction
        {
            LessThan = 1,
            GreaterThan = 2,
            EqualTo = 3,
            GreaterOrEqual = 4,
            LessOrEqual = 5,
        }
    }
    /// <summary>
    /// Used to check if the boolean data in database is equal to the data passed in through constructor
    /// </summary>
    public class BTPreconditionBool : BTPreconditionUseDB
    {
        public bool rhs;
        public BTPreconditionBool(string dataToCheck, bool rhs) : base(dataToCheck)
        {
            this.rhs = rhs;
        }
        public override bool Check()
        {
            bool lhs = database.GetData<bool>(_dataIdToCheck);
            return lhs == rhs;
        }
    }
    /// <summary>
    /// Used to check if the boolean data in database is null
    /// </summary>
    public class BTPreconditionNull : BTPreconditionUseDB
    {
        private NullFunction func;
        public BTPreconditionNull(string dataToCheck, NullFunction func) : base(dataToCheck)
        {
            this.func = func;
        }
        public override bool Check()
        {
            object lhs = database.GetData<object>(_dataIdToCheck);
            switch (func)
            {
                case NullFunction.Null:
                    return lhs == null;
                case NullFunction.NotNull:
                    return lhs != null;
            }
            return false;
        }

        public enum NullFunction
        {
            NotNull = 1,
            Null = 2,
        }
    }
    public class BTAction : BTNode
    {
        private BTActionStatus _status = BTActionStatus.Ready;
        public BTAction(BTPrecondition precondition = null) : base(precondition) { }

        protected virtual void Enter()
        {
            if (MyBT.BTConfiguration.ENABLE_BTACTION_LOG)
            {
                Debug.Log("Enter " + this.name + " [" + this.GetType().ToString() + "]");
            }
        }

        protected virtual void Exit()
        {
            if (MyBT.BTConfiguration.ENABLE_BTACTION_LOG)
            {
                Debug.Log("Exit " + this.name + " [" + this.GetType().ToString() + "]");
            }
        }
        protected virtual BTResult Execute()
        {
            return BTResult.Running;
        }
        public override void Clear()
        {
            if (_status != BTActionStatus.Ready)
            {
                Exit();
                _status = BTActionStatus.Ready;
            }
        }
        public override BTResult Tick()
        {
            BTResult result = BTResult.Ended;
            if (_status == BTActionStatus.Ready)
            {
                Enter();
                _status = BTActionStatus.Running;
            }
            if (_status == BTActionStatus.Running)
            {
                result = Execute();
                if (result != BTResult.Running)
                {
                    Exit();
                    _status = BTActionStatus.Ready;
                }
            }
            return result;
        }
        public override void AddChild(BTNode aNode)
        {
            Debug.LogError("BTAction can't add child");
        }
        public override void RemoveChild(BTNode aNode)
        {
            Debug.LogError("BTAction can't remove child");
        }
        private enum BTActionStatus
        {
            Ready = 1,
            Running = 2,
        }
    }
    /// <summary>
    /// BTParallel evaluates all children, if any of them fails the evaluation, BTParallel fails.
    /// 
    /// BTParallel ticks all children, if 
    /// 	1. ParallelFunction.And: 	ends when all children ends
    /// 	2. ParallelFunction.Or: 	ends when any of the children ends
    /// 
    /// NOTE: Order of child node added does matter!
    /// </summary>
    public class BTParallel : BTNode
    {
        protected List<BTResult> _results;
        protected ParallelFunction _func;
        public BTParallel(ParallelFunction func) : this(func, null) { }
        public BTParallel(ParallelFunction func, BTPrecondition precondition) : base(precondition)
        {
            _results = new List<BTResult>();
            this._func = func;
        }
        protected override bool DoEvaluate()
        {
            foreach (BTNode node in _children)
            {
                if (!node.Evaluate())
                {
                    return false;
                }
            }
            return true;
        }
        public override BTResult Tick()
        {
            int endingResultCount = 0;
            for (int i = 0; i < _children.Count; i++)
            {
                if (_func == ParallelFunction.And)
                {
                    if (_results[i] == BTResult.Running)
                    {
                        _results[i] = _children[i].Tick();
                    }
                    if (_results[i] != BTResult.Running)
                    {
                        endingResultCount++;
                    }
                }
                else
                {
                    if (_results[i] == BTResult.Running)
                    {
                        _results[i] = _children[i].Tick();
                    }
                    if (_results[i] != BTResult.Running)
                    {
                        ResetResults();
                        return BTResult.Ended;
                    }
                }
            }
            if (endingResultCount == _children.Count)
            {
                ResetResults();
                return BTResult.Ended;
            }
            return BTResult.Running;
        }
        public override void Clear()
        {
            ResetResults();
            foreach (BTNode child in _children)
            {
                child.Clear();
            }
        }
        public override void AddChild(BTNode aNode)
        {
            base.AddChild(aNode);
            _results.Add(BTResult.Running);
        }
        public override void RemoveChild(BTNode aNode)
        {
            int index = _children.IndexOf(aNode);
            _results.RemoveAt(index);
            base.RemoveChild(aNode);
        }

        private void ResetResults()
        {
            for (int i = 0; i < _results.Count; i++)
            {
                _results[i] = BTResult.Running;
            }
        }
        public enum ParallelFunction
        {
            And = 1,
            Or = 2,
        }
    }
    public enum CheckType
    {
        Same,
        Different
    }
    public class BTConfiguration
    {
        public static bool ENABLE_DATABASE_LOG = true, ENABLE_BTACTION_LOG = true;
    }
}