using DEAD.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DEAD.Logging
{
    public abstract class ChannelBase<TLevel> : IChannel<TLevel>, IIoCContexted where TLevel : struct
    {


        protected ITargetBlock<LoggingMessage> _startBlock;

        public ChannelBase(string name, ITargetBlock<LoggingMessage> startBlock)
        {
            Name = name;
            _startBlock = startBlock;
        }




        protected abstract Task OnWriteMessageAsync(LoggingMessage message);

        public virtual string Name { get; protected internal set; }
        public abstract long QueueSize { get; }

        public TLevel Level
        {
            get; protected set;
        }

        public IIoCContext IoCContext
        {
            get; set;
        }

        public abstract void Flush();
        public abstract Task FlushAsync();
        public virtual void Log(Action<StringBuilder> buildAction, [CallerMemberName] string member = null, [CallerLineNumber] int line = -1, [CallerFilePath] string filePath = null)
        {
            var sb = new StringBuilder();
            var msg = this.GetIoCManager().DefualtContainer.Resolve<LoggingMessage>();
            msg.StringBuilderAction = buildAction;
            msg.FilePath = filePath;
            msg.Line = line;
            msg.Member = member;
            msg.MessageResult = null;
            msg.Level = LoggerBase<TLevel>.GetOrCreateCachedString(Level);

            _startBlock?.Post(msg);

        }
        public virtual void Log(string message, [CallerMemberName] string member = null, [CallerLineNumber] int line = -1, [CallerFilePath] string filePath = null)
        {

            var msg = this.GetIoCManager().DefualtContainer.Resolve<LoggingMessage>();

            msg.StringBuilderAction = null;
            msg.FilePath = filePath;
            msg.Line = line;
            msg.Member = member;
            msg.MessageResult = message;
            msg.Level = LoggerBase<TLevel>.GetOrCreateCachedString(Level);
            _startBlock?.Post(msg);

        }


    }

    public abstract class DiscretedChannelBase<TLevel> : ChannelBase<TLevel> where TLevel : struct
    {
        public DiscretedChannelBase(string name, int maxDegreeOfParallelism) : base(name, null)
        {
            _startBlock = new ActionBlock<LoggingMessage>(
                OnWriteMessageAsync,
                new ExecutionDataflowBlockOptions
                {
                    EnsureOrdered = true,
                    MaxDegreeOfParallelism = maxDegreeOfParallelism,
                }
                );
        }




        public override long QueueSize { get { return (_startBlock as ActionBlock<LoggingMessage>)?.InputCount ?? 0; } }
    }

    //public class LoggingMessage
    //{
    //    public Action<StringBuilder> StringBuilderAction;
    //    public string MessageResult;
    //    public string Member;
    //    public int Line;
    //    public string FilePath;
    //    public string Level;
    //    public override string ToString()
    //    {

    //        var sb = new StringBuilder();
    //        StringBuilderAction?.Invoke(sb);
    //        sb
    //            .Append("Level:").Append(Level).AppendLine();
    //        if (StringBuilderAction == null)
    //        {
    //            sb
    //                .AppendLine("Message:")
    //                .AppendLine(MessageResult);
    //        }

    //        sb
    //            .AppendFormat("calling member {0}, line {1}, file: {2}", Member, Line, FilePath)
    //            .AppendLine();
    //        MessageResult = sb.ToString();
    //        StringBuilderAction = null;

    //        return MessageResult;
    //    }
    //}
}
