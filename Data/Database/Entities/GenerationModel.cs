using Data.Database.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Database.Entities
{
    [Table("generations")]
    public class GenerationModel : BaseModel
    {
        [Column("prompt")]
        [MaxLength(ushort.MaxValue)]
        public string Prompt { get; set; }

        [Column("responce")]
        [MaxLength(ushort.MaxValue)]
        public string Responce { get; set; }

        [Column("model")]
        [MaxLength(byte.MaxValue)]
        public string Model { get; set; }

        [Column("completion_type")]
        [MaxLength(byte.MaxValue)]
        public string CompletionType { get; set; }

        [Column("temperature")]
        public float Temperature { get; set; }

        [Column("repetition_penalty")]
        public float RepetitionPenalty { get; set; }

        [Column("top_p")]
        public float TopP { get; set; }

        [Column("prompt_tokens")]
        public ushort PromptTokens { get; set; }

        [Column("completion_tokens")]
        public ushort CompletionTokens { get; set; }
    }
}
