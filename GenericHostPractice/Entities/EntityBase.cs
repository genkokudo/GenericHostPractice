using System;

namespace GenericHostPractice.Entities
{
    /// <summary>
    /// 登録日時、更新日時
    /// </summary>
    public class EntityBase
    {
        /// <summary>
        /// 登録者
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        public DateTime? UpdatedDate { get; set; }
    }
}
