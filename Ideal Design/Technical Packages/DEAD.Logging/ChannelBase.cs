using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DEAD.Logging
{
    public abstract class ChannelBase<TLevel> : IChannel<TLevel> where TLevel : struct
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

        public abstract void Flush();
        public abstract Task FlushAsync();
        public virtual void Log(Action<StringBuilder> buildAction, [CallerMemberName] string member = null, [CallerLineNumber] int line = -1, [CallerFilePath] string filePath = null)
        {
            var sb = new StringBuilder();
            _startBlock?.Post(new LoggingMessage
            {
                StringBuilderAction = buildAction,
                FilePath = filePath,
                Line = line,
                Member = member,
                MessageResult = null,
                Level = LoggerBase<TLevel>.GetOrCreateCachedString(Level)
            });

        }
        public virtual void Log(string message, [CallerMemberName] string member = null, [CallerLineNumber] int line = -1, [CallerFilePath] string filePath = null)
        {
            _startBlock?.Post(new LoggingMessage
            {
                StringBuilderAction = null,
                FilePath = filePath,
                Line = line,
                Member = member,
                MessageResult = message,
                Level = LoggerBase<TLevel>.GetOrCreateCachedString(Level)
            });

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

    public struct LoggingMessage
    {
        public Action<StringBuilder> StringBuilderAction;
        public string MessageResult;
        public string Member;
        public int Line;
        public string FilePath;
        public string Level;
        public string GetOrBuildMessage()
        {

            var sb = new StringBuilder();
            StringBuilderAction?.Invoke(sb);
            sb
                .Append("Level:").Append(Level).AppendLine()
                .AppendFormat("calling member {0}, line {1}, file: {2}", Member, Line, FilePath)
                .AppendLine();
            MessageResult = sb.ToString();
            StringBuilderAction = null;

            return MessageResult;
        }
    }
}
