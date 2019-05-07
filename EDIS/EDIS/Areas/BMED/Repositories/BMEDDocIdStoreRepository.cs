﻿using System;
using System.Collections.Generic;
using System.Linq;
using EDIS.Areas.BMED.Data;
using EDIS.Models.Identity;
using EDIS.Areas.BMED.Models;

namespace EDIS.Areas.BMED.Repositories
{
    public class BMEDDocIdStoreRepository : BMEDIRepository<DocIdStore, string[]>
    {
        private readonly BMEDDbContext _context;

        public BMEDDocIdStoreRepository(BMEDDbContext context)
        {
            _context = context;
        }

        public string[] Create(DocIdStore entity)
        {
            _context.BMEDDocIdStore.Add(entity);
            _context.SaveChanges();

            return new string[] { entity.DocType, entity.DocId };
        }

        public void Delete(string[] id)
        {
            _context.Remove(_context.BMEDDocIdStore.Single(x => x.DocType == id[0] && x.DocId == id[1]));
            _context.SaveChanges();
        }

        public IEnumerable<DocIdStore> Find(System.Linq.Expressions.Expression<Func<DocIdStore, bool>> expression)
        {
            return _context.BMEDDocIdStore.Where(expression);
        }

        public DocIdStore FindById(string[] id)
        {
            return _context.BMEDDocIdStore.SingleOrDefault(x => x.DocType == id[0] && x.DocId == id[1]);
        }

        public void Update(DocIdStore entity)
        {
            var oriDpt = _context.BMEDDocIdStore.Single(x => x.DocType == entity.DocType);
            Delete(new string[] { oriDpt.DocType, oriDpt.DocId });
            Create(entity);
        }
    }
}
