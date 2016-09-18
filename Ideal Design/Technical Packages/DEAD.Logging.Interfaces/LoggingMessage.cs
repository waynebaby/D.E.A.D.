using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.Logging
{

    public class LoggingMessage 
    {
        public Action<StringBuilder> StringBuilderAction { get; set; }
        public string MessageResult { get; set; }
        public string Member { get; set; }
        public int Line { get; set; }
        public string FilePath { get; set; }
        public string Level { get; set; }
        public override string ToString()
        {

            var sb = new StringBuilder();
            StringBuilderAction?.Invoke(sb);
            sb
                .Append("Level:").Append(Level).AppendLine();
            if (StringBuilderAction == null)
            {
                sb
                    .AppendLine("Message:")
                    .AppendLine(MessageResult);
            }

            sb
                .AppendFormat("calling member {0}, line {1}, file: {2}", Member, Line, FilePath)
                .AppendLine();
            MessageResult = sb.ToString();
            StringBuilderAction = null;

            return MessageResult;
        }
    }
}
