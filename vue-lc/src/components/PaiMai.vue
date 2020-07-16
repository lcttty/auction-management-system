<template>
  <div style="margin-top:30px">
    <div><hr style="height:3px;border:none;border-top:3px solid #FF9797 ;"/></div>
    <el-breadcrumb separator=">">
      <el-breadcrumb-item :to="{path: '/paimai/all'}">首页</el-breadcrumb-item>
      <el-breadcrumb-item :to="{path: '/paimai/zghh'}" v-if="a.a1">中国绘画</el-breadcrumb-item>
      <el-breadcrumb-item :to="{path: '/paimai/sfzk'}" v-if="a.a2">书法篆刻</el-breadcrumb-item>
      <el-breadcrumb-item :to="{path: '/paimai/xhds'}" v-if="a.a3">西画雕塑</el-breadcrumb-item>
      <el-breadcrumb-item :to="{path: '/paimai/gczx'}" v-if="a.a4">古瓷杂项</el-breadcrumb-item>
      <el-breadcrumb-item :to="{path: '/paimai/ddgy'}" v-if="a.a5">当代工艺</el-breadcrumb-item>
      <el-breadcrumb-item :to="{path: '/paimai/jp'}" v-if="a.a6">酒品</el-breadcrumb-item>
    </el-breadcrumb>
    <el-tabs v-model="activeName" @tab-click="handleClick" style="margin-top:10px">
        <el-tab-pane label="全部" name="first">
        <el-card shadow="never" style="margin-top:30px; width:100%;">
          <el-row>
            <el-col :span="6" v-for="tt in table.slice((currentPage - 1) * 16, currentPage * 16)" :key="tt.name">
              <el-link class="link" :underline="false" @click="link(tt.name)">
              <el-card>
                <img :src="tt.url" class="image"/>
                <br/><br/>
                <div style="text-align:left">
                  <span>
                    <b style="font-size:120%" v-if="tt.name.length>14">{{tt.name.substr(0,12)}}...</b>
                    <b style="font-size:120%" v-else>{{tt.name}}</b>
                  </span>
                </div>
                <div style="text-align:left">
                  <span>当前价:<b style="color:red; font-size:140%;margin-left:3%">￥{{tt.nowprice}}</b></span>
                </div>
                <div style="text-align:left">
                  <span>下线时间:{{tt.endtime.replace("T", " ")}}</span>
                </div>
              </el-card>
              </el-link>
            </el-col>
          </el-row>
        </el-card>
        </el-tab-pane>
        <el-tab-pane label="未结束拍卖" name="second">
        <el-card shadow="never" style="margin-top:30px; width:100%;">
          <el-row>
            <el-col :span="6" v-for="tt in table1.slice((currentPage - 1) * 16, currentPage * 16)" :key="tt.name">
              <el-link class="link" :underline="false" @click="link(tt.name)">
              <el-card>
                <img :src="tt.url" class="image"/>
                <br/><br/>
                <div style="text-align:left">
                  <span>
                    <b style="font-size:120%" v-if="tt.name.length>14">{{tt.name.substr(0,12)}}...</b>
                    <b style="font-size:120%" v-else>{{tt.name}}</b>
                  </span>
                </div>
                <div style="text-align:left">
                  <span>当前价:<b style="color:red; font-size:140%;margin-left:3%">￥{{tt.nowprice}}</b></span>
                </div>
                <div style="text-align:left">
                  <span>下线时间:{{tt.endtime.replace("T", " ")}}</span>
                </div>
              </el-card>
              </el-link>
            </el-col>
          </el-row>
        </el-card>
        </el-tab-pane>
        <el-tab-pane label="已结束拍卖" name="third">
        <el-card shadow="never" style="margin-top:30px; width:100%;">
          <el-row>
            <el-col :span="6" v-for="tt in table2.slice((currentPage - 1) * 16, currentPage * 16)" :key="tt.name">
              <el-link class="link" :underline="false" @click="link(tt.name)">
              <el-card>
                <img :src="tt.url" class="image"/>
                <br/><br/>
                <div style="text-align:left">
                  <span>
                    <b style="font-size:120%" v-if="tt.name.length>14">{{tt.name.substr(0,12)}}...</b>
                    <b style="font-size:120%" v-else>{{tt.name}}</b>
                  </span>
                </div>
                <div style="text-align:left">
                  <span>当前价:<b style="color:red; font-size:140%;margin-left:3%">￥{{tt.nowprice}}</b></span>
                </div>
                <div style="text-align:left">
                  <span>下线时间:{{tt.endtime.replace("T", " ")}}</span>
                </div>
              </el-card>
              </el-link>
            </el-col>
          </el-row>
        </el-card>
        </el-tab-pane>
    </el-tabs>
    <el-pagination layout="prev, pager, next" :total="cc" @current-change="current_change">
    </el-pagination>
  </div>
</template>
<script>
export default {
  name: 'PaiMai',
  data () {
    return {
      a: {
        a1: false,
        a2: false,
        a3: false,
        a4: false,
        a5: false,
        a6: false
      },
      count: 0,
      count1: 0,
      count2: 0,
      cc: 0,
      currentPage: 1,
      currentPage1: 1,
      currentPage2: 1,
      table: [],
      table1: [],
      table2: [],
      activeName: 'first'
    }
  },
  methods: {
    start (query) {
      switch (query) {
        case 'all':
          this.quanfalse(this.a)
          break
        case 'zghh':
          this.quanfalse(this.a)
          this.a.a1 = true
          break
        case 'sfzk':
          this.quanfalse(this.a)
          this.a.a2 = true
          break
        case 'xhds':
          this.quanfalse(this.a)
          this.a.a3 = true
          break
        case 'gczx':
          this.quanfalse(this.a)
          this.a.a4 = true
          break
        case 'ddgy':
          this.quanfalse(this.a)
          this.a.a5 = true
          break
        case 'jp':
          this.quanfalse(this.a)
          this.a.a6 = true
          break
      }
      var ur = 'product/pmpquery/' + query
      this.$axios({method: 'post', url: ur})
        .then(response => {
          this.table = response.data.Table
          this.table1 = []
          this.table2 = []
          this.count = Math.ceil((this.table.length / 16) * 10)
          var date = new Date().getTime()
          for (var tt in this.table) {
            var time1 = this.table[tt].endtime.split('T')
            var time2 = time1[0].split('-')
            var time3 = time1[1].split(':')
            var date1 = new Date()
            date1.setFullYear(time2[0], time2[1] - 1, time2[2])
            date1.setHours(time3[0])
            date1.setMinutes(time3[1])
            date1.setSeconds(time3[2])
            var date2 = date1.getTime()
            if (date2 < date) {
              this.table2.push(this.table[tt])
            } else {
              this.table1.push(this.table[tt])
            }
          }
          this.count1 = Math.ceil((this.table1.length / 16) * 10)
          this.count2 = Math.ceil((this.table2.length / 16) * 10)
          this.cc = this.count
          this.activeName = 'first'
        })
    },
    quanfalse (a) {
      for (var aa in a) {
        a[aa] = false
      }
    },
    current_change (currentPage) {
      this.currentPage = currentPage
      this.currentPage1 = currentPage
      this.currentPage2 = currentPage
    },
    handleClick (tab) {
      this.currentPage = 1
      this.currentPage1 = 1
      this.currentPage2 = 1
      if (tab.paneName === 'first') {
        this.cc = this.count
      }
      if (tab.paneName === 'second') {
        this.cc = this.count1
      }
      if (tab.paneName === 'third') {
        this.cc = this.count2
      }
    },
    link (name) {
      let routeUrl = this.$router.resolve({path: '/buy', query: {name: name, method: 'putong'}})
      window.open(routeUrl.href, '_blank')
    }
  },
  mounted: function () {
    var query = this.$route.params.query
    var myQuery = ['all', 'zghh', 'sfzk', 'xhds', 'gczx', 'ddgy', 'jp']
    if (myQuery.indexOf(query) === -1) {
      query = 'all'
    }
    this.start(query)
  },
  watch: {
    $route (to, from) {
      var query = to.params.query
      var myQuery = ['all', 'zghh', 'sfzk', 'xhds', 'gczx', 'ddgy', 'jp']
      if (myQuery.indexOf(query) === -1) {
        query = 'all'
      }
      this.currentPage = 1
      this.currentPage1 = 1
      this.currentPage2 = 1
      this.start(query)
    }
  }
}
</script>
<style scoped>
.link:visited .link:hover .link:link .link:active {
  color: black;
}
</style>
