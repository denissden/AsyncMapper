using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AsyncMapper.Exceptions
{
    public class MappingException : Exception
    {
        TypePair? types;
        //string message;
        public MappingException() : base()
        {
        }

        public MappingException(TypePair typePair, string message) : base(message)
        {
            types = typePair;
        }

        public MappingException(string message) : base(message)
        {
            //this.message = message;
        }

        public MappingException(string message, Exception innerException) : base(message, innerException)
        {
            //this.message = message;
        }

        public override string Message
        {
            get { 
                var message = new StringBuilder();
                if (types != null) message.AppendLine($"Cannot map from {types?.SourceType.Name} to {types?.DestinationType.Name}.");
                message.AppendLine(base.Message);
                return message.ToString();
            }    
        }
    }
}
