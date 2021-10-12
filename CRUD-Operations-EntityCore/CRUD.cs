using System;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Operations_EntityCore
{
    public class CRUD<O> : CRUD_Operations.CRUDDefault<O>
    {
        public CRUD()
        {
            
            readList();
        }

        public override void readList()
        {
            
        }
        public override void safeList()
        {
            
        }
    }
}
