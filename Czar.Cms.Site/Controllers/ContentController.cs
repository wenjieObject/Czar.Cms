using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Czar.Cms.Models;
using Czar.Cms.IRepositories;
using System.Threading.Tasks;
using Czar.Cms.Models;
using System.Linq;

namespace Czar.Cms.Controllers
{
    public class ContentController : Controller
    {
        private IContentRepository ContentRepository;
        
        public ContentController(IContentRepository repository)
        {
            ContentRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        #region Views

		public IActionResult Index()
        {
            return View();
        }

        #endregion

        #region Methods

        [AjaxRequestOnly, HttpGet]
        public Task<IActionResult> GetEntities()
        {
            return Task.Factory.StartNew<IActionResult>(() =>
            {
                    var rows = ContentRepository.Get().ToList();
                    return Json(ExcutedResult.SuccessResult(rows));
            });
        }

        [AjaxRequestOnly]
        public Task<IActionResult> GetEntitiesByPaged(int pageSize, int pageIndex)
        {
            return Task.Factory.StartNew<IActionResult>(() =>
            {
                var total = ContentRepository.Count(m => true);
                var rows = ContentRepository.GetByPagination(m => true, pageSize, pageIndex, true,
                    m => m.Id).ToList();
                return Json(PaginationResult.PagedResult(rows, total, pageSize, pageIndex));
            });
        }
        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AjaxRequestOnly,HttpPost,ValidateAntiForgeryToken]
        public Task<IActionResult> Add(Content model)
        {
            return Task.Factory.StartNew<IActionResult>(() =>
            {
                if(!ModelState.IsValid)
                    return Json(ExcutedResult.FailedResult("数据验证失败"));
                ContentRepository.AddAsync(model, false);
                return Json(ExcutedResult.SuccessResult());
            });
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AjaxRequestOnly, HttpPost]
        public Task<IActionResult> Edit(Content model)
        {
            return Task.Factory.StartNew<IActionResult>(() =>
            {
                if (!ModelState.IsValid)
                    return Json(ExcutedResult.FailedResult("数据验证失败"));
                ContentRepository.Edit(model, false);
                return Json(ExcutedResult.SuccessResult());
            });
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxRequestOnly]
        public Task<IActionResult> Delete(int id)
        {
            return Task.Factory.StartNew<IActionResult>(() =>
            {
                ContentRepository.Delete(id, false);
                return Json(ExcutedResult.SuccessResult("成功删除一条数据。"));
            });
        }

        #endregion
	}
}