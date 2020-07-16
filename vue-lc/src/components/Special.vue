<template>
  <div style="margin-top: 30px">
    <el-card>
      <div slot="header"><span>专场名称</span></div>
      <el-row>
      <el-col :span="6" v-for="dd in ds" :key="dd.zname" style="margin-top: 30px">
        <el-link :underline="false" @click="xunzhao(dd.zname)"><span style="color:red">{{dd.zname}}</span></el-link>
      </el-col>
      </el-row>
    </el-card>
    <div><hr style="height:3px;border:none;border-top:3px solid #FF9797;margin-top:30px"/></div>
    <div v-if="active">
    <el-card>
      <div>专场名称: {{zname}}</div>
      <div>本场负责人: {{table[0].fzperson}}</div>
    </el-card>
    <el-tabs v-model="activeName" @tab-click="handleClick" style="margin-top:10px">
        <el-tab-pane label="全部" name="first">
        <el-card shadow="never" style="margin-top:30px; width:100%;">
          <el-row>
            <el-col :span="6" v-for="tt in table.slice((currentPage - 1) * 16, currentPage * 16)" :key="tt.url">
              <el-link class="link" :underline="false" @click="link(tt.name, tt.zname)">
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
            <el-col :span="6" v-for="tt in table1.slice((currentPage - 1) * 16, currentPage * 16)" :key="tt.url">
              <el-link class="link" :underline="false" @click="link(tt.name, tt.zname)">
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
            <el-col :span="6" v-for="tt in table2.slice((currentPage - 1) * 16, currentPage * 16)" :key="tt.url">
              <el-link class="link" :underline="false" @click="link(tt.name, tt.zname)">
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
  </div>
</template>
<script>
export default {
  name: 'Special',
  data () {
    return {
      zname: '',
      ds: [],
      table: [],
      table1: [],
      table2: [],
      count: 0,
      tcount1: 0,
      tcount2: 0,
      tcount3: 0,
      cc: 0,
      activeName: 'first',
      currentPage: 1,
      currentPage1: 1,
      currentPage2: 1,
      active: false,
      name: ''
    }
  },
  methods: {
    zhuanchang () {
      this.$axios({method: 'get', url: 'product/zhuanchang'})
        .then(response => {
          this.ds = response.data.Table
          this.count = this.ds.length
        })
    },
    zhuanchangquery (query) {
      this.activeName = query
      query = encodeURIComponent(query)
      this.$axios({method: 'post', url: 'product/zhuanchang/' + query})
        .then(response => {
          this.table = response.data.Table
          this.table1 = []
          this.table2 = []
          this.tcount1 = Math.ceil((this.table.length / 16) * 10)
          this.active = true
          this.activeName = 'first'
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
          this.tcount2 = Math.ceil((this.table1.length / 16) * 10)
          this.tcount3 = Math.ceil((this.table2.length / 16) * 10)
          this.cc = this.tcount1
        })
    },
    xunzhao (name) {
      this.zname = name
      this.zhuanchangquery(name)
    },
    handleClick (tab) {
      this.currentPage = 1
      this.currentPage1 = 1
      this.currentPage2 = 1
      if (tab.paneName === 'first') {
        this.cc = this.tcount1
      }
      if (tab.paneName === 'second') {
        this.cc = this.tcount2
      }
      if (tab.paneName === 'third') {
        this.cc = this.tcount3
      }
    },
    link (name, zname) {
      let routeUrl = this.$router.resolve({path: '/buy', query: {name: name, zname: zname, method: 'zhuanchang'}})
      window.open(routeUrl.href, '_blank')
    }
  },
  created: function () {
    this.zhuanchang()
    this.name = this.$route.query.name
    if (this.name !== '' && this.name !== undefined && this.name !== null) {
      this.zhuanchangquery(this.name)
    }
  },
  watch: {
  }
}
</script>
<style scoped>
a {
  text-decoration: none;
}
</style>
