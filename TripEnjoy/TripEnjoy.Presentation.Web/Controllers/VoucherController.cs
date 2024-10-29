using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TripEnjoy.Application.Interface.Voucher;

namespace TripEnjoy.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;

        public VoucherController(IVoucherService voucherService)
        {
            this._voucherService = voucherService;
        }

        [HttpGet]
        [Route("GetVouchers")]
        public async Task<IActionResult> GetVouchers()
        {
            var voucherList =  _voucherService.GetVouchersAsync();
            return Ok(voucherList);
        }

    }
}
