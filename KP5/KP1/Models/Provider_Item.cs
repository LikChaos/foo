//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KP1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Provider_Item
    {
        public int Id { get; set; }
        public int Id_item { get; set; }
        public int Id_provider { get; set; }
    
        public virtual Item Item { get; set; }
        public virtual Provider Provider { get; set; }
    }
}
