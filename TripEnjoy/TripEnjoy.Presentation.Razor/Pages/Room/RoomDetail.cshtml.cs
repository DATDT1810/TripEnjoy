using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using TripEnjoy.Presentation.Razor.ViewModels;

namespace TripEnjoy.Presentation.Razor.Pages.Room
{
    public class RoomDetailModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty(SupportsGet = true)]
        public RoomDetail RoomDetail { get; set; }

        [BindProperty(SupportsGet = true)]
        public IEnumerable<RoomImages> RoomImages { get; set; }
        [BindProperty(SupportsGet = true)]
        public IEnumerable<RoomDetail> RelatedRooms { get; set; }

        [BindProperty(SupportsGet = true)]
        public IEnumerable<Rate> Rates { get; set; }

        [BindProperty(SupportsGet = true)]
        public IEnumerable<Comment> Comments { get; set; }

        [BindProperty(SupportsGet = true)]
        public RateAndComment RateAndComment { get; set; } = new RateAndComment();

        [BindProperty(SupportsGet = true)]
        public CommentReply CommentReply { get; set; } = new CommentReply();

        [BindProperty(SupportsGet = true)]
        public BookingNoSearchVM BookingNoSearchVM { get; set; }

        public RoomDetailModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            HttpContext.Session.SetInt32("RoomId", id);
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.GetAsync($"https://localhost:7126/api/Room/{id}");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                RoomDetail = JsonSerializer.Deserialize<RoomDetail>(data, option);

                // Goi API lay list image cua phong
                var imagesResponse = await client.GetAsync($"https://localhost:7126/api/RoomImage/images/{id}");
                if (imagesResponse.IsSuccessStatusCode)
                {
                    string imagesData = await imagesResponse.Content.ReadAsStringAsync();
                    RoomImages = JsonSerializer.Deserialize<List<RoomImages>>(imagesData, option);
                }

                // Gọi API để lấy danh sách phòng liên quan
                var relatedRoomsResponse = await client.GetAsync($"https://localhost:7126/api/Room/related/{RoomDetail.RoomTypeId}/{RoomDetail.HotelId}");
                if (relatedRoomsResponse.IsSuccessStatusCode)
                {
                    string relatedRoomsData = await relatedRoomsResponse.Content.ReadAsStringAsync();
                    RelatedRooms = JsonSerializer.Deserialize<List<RoomDetail>>(relatedRoomsData, option);
                    // loại bỏ phòng hiện tại ra
                    RelatedRooms = RelatedRooms.Where(room => room.RoomId != id).ToList();

                    // Lấy ảnh cho từng phòng trong danh sách RelatedRooms
                    foreach (var room in RelatedRooms)
                    {
                        var roomImagesResponse = await client.GetAsync($"https://localhost:7126/api/RoomImage/images/{room.RoomId}");
                        if (roomImagesResponse.IsSuccessStatusCode)
                        {
                            string roomImagesData = await roomImagesResponse.Content.ReadAsStringAsync();
                            var roomImages = JsonSerializer.Deserialize<List<RoomImages>>(roomImagesData, option);
                            room.RoomImages = roomImages?.Take(1).ToList();
                        }
                    }
                }

                // Lấy danh sách đánh giá
                var rateRequest = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7126/api/Rate/GetRatesForRoom/" + id );
                var ratesResponse = await client.SendAsync(rateRequest);
                if (ratesResponse.IsSuccessStatusCode)
                {
                    string ratesData = await ratesResponse.Content.ReadAsStringAsync();
                    Rates = JsonSerializer.Deserialize<List<Rate>>(ratesData, option); //3 lượt rate trong phòng này
                }

                
                var commentsResponse = await client.GetAsync($"https://localhost:7126/api/Comment/Room/{id}");
                if (commentsResponse.IsSuccessStatusCode)
                {
                    string commentsData = await commentsResponse.Content.ReadAsStringAsync();
                    Comments = JsonSerializer.Deserialize<List<Comment>>(commentsData, option); // 5 comment trong phòng
             
                    var ratesList = Rates.ToList(); // chuyển đổi cho việc sử dụng for
                    var rateIndex = 0; // start = 0 -> rate ++
                    foreach (var item in Comments)
                    {
                        if (item.CommentReply == "0" && rateIndex < ratesList.Count)
                        {
                            item.RateValue = ratesList[rateIndex].RateValue;
                            rateIndex++;
                        }
                    }
                }

                return Page();
            }
            else
            {
                var redirectPath = await response.Content.ReadAsStringAsync();
                return RedirectToPage(redirectPath);
            }

           
        }

        public async Task<IActionResult> OnPostReplyAsync()
        {
            if (CommentReply == null || string.IsNullOrWhiteSpace(CommentReply.Content))
            {
                ModelState.AddModelError(string.Empty, "Reply cannot be empty.");
                return Page();
            }

            var client = _httpClientFactory.CreateClient("DefaultClient");
            var replyComment = new
            {
                RoomId = CommentReply.RoomId,
                Content = CommentReply.Content,
                ReplyToComment = CommentReply.ReplyToComment.ToString()
            };

            string data = JsonSerializer.Serialize(replyComment);
            var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:7126/api/Comment/Reply", content);

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Login");
            }
            return RedirectToPage(new { id = CommentReply.RoomId });
        }

        
        public async Task<IActionResult> OnPostRateAndCommentAsync()
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");

            // Create a RateAndCommentDTO payload based on the RateAndComment model
            var rateAndCommentDTO = new
            {
                RoomId = RateAndComment.RoomId,
                RateValue = RateAndComment.RateValue,
                CommentContent = RateAndComment.CommentContent,
                ReviewDate = DateTime.Now
            };


            // rate and comment
            var content = new StringContent(JsonSerializer.Serialize(rateAndCommentDTO), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7126/api/Rate/RateAndComment", content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error when sending rate and comment");
            }

            return new JsonResult(new { success = true, message = "Rate and comment added successfully!" });
        }


        public async Task<IActionResult> OnPostBookingNoSearch()
        {
            if (BookingNoSearchVM != null)
            {
                // Kiểm tra và lấy dữ liệu từ form hoặc session
                var checkinDate = BookingNoSearchVM?.Checkin ?? DateTime.Parse(HttpContext.Session.GetString("CheckinDate") ?? DateTime.Now.ToString());
                var checkoutDate = BookingNoSearchVM?.Checkout ?? DateTime.Parse(HttpContext.Session.GetString("CheckoutDate") ?? DateTime.Now.AddDays(1).ToString());
                var quantity = BookingNoSearchVM?.Quantity ?? HttpContext.Session.GetInt32("RoomQuantity") ?? 1;

                // Cập nhật lại session dựa trên form
                HttpContext.Session.SetString("CheckinDate", checkinDate.ToString());
                HttpContext.Session.SetString("CheckoutDate", checkoutDate.ToString());
                HttpContext.Session.SetInt32("RoomQuantity", quantity);
            }
            return RedirectToPage("/Booking/Booking");
        }

    }
}
