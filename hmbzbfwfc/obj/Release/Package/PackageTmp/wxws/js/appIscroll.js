/** 
       * 初始化iScroll控件 
       */

function loaded() {
    pullDownEl = document.getElementById('pullDown');
    pullDownOffset = pullDownEl.offsetHeight;
    pullUpEl = document.getElementById('pullUp');
    pullUpOffset = pullUpEl.offsetHeight;

        myScroll = new iScroll('wrapper', {
          //  scrollbarClass: 'myScrollbar', /* 重要样式 */
            useTransition: true,//是否使用CSS变换
            topOffset: pullDownOffset-10,
            hScroll: true,
            vScroll: true,
            hScrollbar: false,
            vScrollbar: true,
            fixedScrollbar: true,
            fadeScrollbar: true,
            hideScrollbar: true,
            bounce: true,
            momentum: true,
            lockDirection: true,
            checkDOMChanges: true,
            onRefresh: function () {
                if (pullDownEl.className.match('loading')) {
                    pullDownEl.className = '';
                    pullDownEl.querySelector('.pullDownLabel').innerHTML = '下拉刷新...';
                } else if (pullUpEl.className.match('loading')) {
                    pullUpEl.className = '';
                    pullUpEl.querySelector('.pullUpLabel').innerHTML = '上拉加载更多...';
                    pullUpEl.style.display = 'none';
                }
            },
            onScrollMove: function () {
                //开始滚动回调
                if (this.y > 15 && !pullDownEl.className.match('flip')) {
                    pullDownEl.className = 'flip';
                    pullDownEl.querySelector('.pullDownLabel').innerHTML = '松手开始更新...';
                    this.minScrollY = 0;
                } else if (this.y < 15 && pullDownEl.className.match('flip')) {
                    pullDownEl.className = '';
                    pullDownEl.querySelector('.pullDownLabel').innerHTML = '下拉刷新...';
                    this.minScrollY = -pullDownOffset;
                } else if (this.y < (this.maxScrollY - 15) && !pullUpEl.className.match('flip')) {
                    pullUpEl.className = 'flip';
                    pullUpEl.querySelector('.pullUpLabel').innerHTML = '松手开始更新...';
                    pullUpEl.style.display = 'block';
                    this.maxScrollY = this.maxScrollY;
                } else if (this.y > (this.maxScrollY + 15) && pullUpEl.className.match('flip')) {
                    pullUpEl.className = '';
                    pullUpEl.querySelector('.pullUpLabel').innerHTML = '上拉加载更多...';
                    pullUpEl.style.display = 'block';
                    this.maxScrollY = pullUpOffset;
                }
            },
            onScrollEnd: function () {//手离开后回调
                if (pullDownEl.className.match('flip')) {
                    pullDownEl.className = 'loading';
                    pullDownEl.querySelector('.pullDownLabel').innerHTML = '加载中...';
                    pullDownAction(); // Execute custom function (ajax call?) 
                } else if (pullUpEl.className.match('flip')) {
                    pullUpEl.className = 'loading';
                    pullUpEl.querySelector('.pullUpLabel').innerHTML = '加载中...';
                    pullUpAction(); // Execute custom function (ajax call?) 
                }
            }
        });

   setTimeout(function () { document.getElementById('wrapper').style.left = '0'; }, 300);
}
//初始化绑定iScroll控件  

document.addEventListener('touchmove', function (e) { e.preventDefault(); }, false);

    document.addEventListener('DOMContentLoaded', loaded, false);
