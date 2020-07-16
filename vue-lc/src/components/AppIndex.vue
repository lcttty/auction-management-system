<template>
  <div>
    <el-carousel height="450px" indicator-position="outside">
      <el-carousel-item v-for="item in urls" :key="item.zname">
        <el-link :underline="false" @click="zlink(item.zname)"><img :src="item.url"/></el-link>
      </el-carousel-item>
    </el-carousel>
    <div><h3 style="text-align:left;margin-left:25px">精品专场</h3></div>
    <el-row>
      <el-col :span="6" v-for="jj in jpzc" :key="jj.zname">
        <el-link class="link" :underline="false" @click="zlink(jj.zname)">
        <el-card>
          <img :src="jj.url"/>
          <div style="padding: 14px;">
            <span>{{jj.zname}}</span>
          </div>
        </el-card>
        </el-link>
      </el-col>
    </el-row>
    <div><h3 style="text-align:left;margin-left:25px">中国绘画</h3></div>
    <el-row>
      <el-col :span="6" v-for="k in zghh" :key="k.name">
        <el-link class="link" :underline="false" @click="link(k.name)">
        <el-card>
          <img :src="k.url"/>
          <div style="padding: 14px;">
            <span>{{k.name}}</span>
          </div>
        </el-card>
        </el-link>
      </el-col>
    </el-row>
    <div><h3 style="text-align:left;margin-left:25px">古瓷杂项</h3></div>
    <el-row>
      <el-col :span="6" v-for="j in gczx" :key="j.name">
        <el-link class="link" :underline="false" @click="link(j.name)">
        <el-card>
          <img :src="j.url"/>
          <div style="padding: 14px;">
            <span v-if="j.name.length>15">{{j.name.substr(0,15)}}...</span>
            <span v-else>{{j.name}}</span>
          </div>
        </el-card>
        </el-link>
      </el-col>
    </el-row>
    <div><h3 style="text-align:left;margin-left:25px">红酒</h3></div>
    <el-row>
      <el-col :span="6" v-for="i in hj" :key="i.name">
        <el-link class="link" :underline="false" @click="link(i.name)">
        <el-card>
          <img :src="i.url"/>
          <div style="padding: 14px;">
            <span v-if="i.name.length>15">{{i.name.substr(0,15)}}...</span>
            <span v-else>{{i.name}}</span>
          </div>
        </el-card>
        </el-link>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="12">
        <div>
          <h3 style="text-align:left;margin-left:25px">即将开拍</h3>
          <ul v-for="jj in jjs" :key="jj.name" style="text-align:left;">
            <el-link class="link" :underline="false" @click="link(jj.name)" style="padding:left">
            <li style="text-align:left">
              <span v-if="jj.name.length>25">{{ jj.name.substr(0, 25) }}...</span>
              <span v-else>{{jj.name}}</span>
              <span style="padding: 30px;color:red">{{jj.starttime}}</span></li>
            </el-link>
          </ul>
        </div>
      </el-col>
      <el-col :span="12">
        <div>
          <h3 style="text-align:left;margin-left:25px">最新出价</h3>
          <ul v-for="zx in zxs" :key="zx.number" style="text-align:left;">
            <el-link class="link" :underline="false" @click="linkzx(zx)" style="padding:left">
            <li>
              <span v-if="zx.name.length>15">{{ zx.name.substr(0, 20) }}...</span>
              <span v-else>{{zx.name}}</span>
              <span style="padding: 30px;color:red">{{'￥' + zx.price}}</span></li>
            </el-link>
          </ul>
        </div>
      </el-col>
    </el-row>
  </div>
</template>
<script>
export default {
  name: 'AppIndex',
  data () {
    return {
      urls: [],
      jpzc: [],
      zghh: [],
      gczx: [],
      hj: [],
      jjs: [],
      zxs: []
    }
  },
  methods: {
    getjj () {
      this.$axios({method: 'post', url: 'product/getjj'})
        .then(response => {
          var table = response.data.Table
          for (var i in table) {
            if (table[i] !== null) {
              table[i].starttime = table[i].starttime.replace('T', ' ')
              this.jjs.push(table[i])
            }
          }
        })
    },
    getzx () {
      this.$axios({method: 'post', url: 'product/getzx'})
        .then(response => {
          var table = response.data
          for (var i = 0; i < table.length; i++) {
            this.zxs.push(table[i])
          }
          console.log(this.zxs)
        })
    },
    gethj () {
      this.$axios({method: 'post', url: 'product/gethj'})
        .then(response => {
          var table = response.data.Table
          for (var i in table) {
            this.hj.push(table[i])
          }
        })
    },
    getgczx () {
      this.$axios({method: 'post', url: 'product/getgczx'})
        .then(response => {
          var table = response.data.Table
          for (var i in table) {
            this.gczx.push(table[i])
          }
        })
    },
    getzghh () {
      this.$axios({method: 'post', url: 'product/getzghh'})
        .then(response => {
          var table = response.data.Table
          for (var i in table) {
            this.zghh.push(table[i])
          }
        })
    },
    getjpzc () {
      this.$axios({method: 'post', url: 'product/getjpzc'})
        .then(response => {
          var table = response.data.Table
          for (var i in table) {
            this.jpzc.push(table[i])
          }
        })
    },
    geturls () {
      this.$axios({method: 'post', url: 'product/geturls'})
        .then(response => {
          var table = response.data.Table
          for (var i in table) {
            this.urls.push(table[i])
          }
        })
    },
    linkzx (zx) {
      if (zx.state === '0') {
        let routeUrl = this.$router.resolve({path: '/buy', query: {name: zx.name, method: 'putong'}})
        window.open(routeUrl.href, '_blank')
      } else {
        let routeUrl = this.$router.resolve({path: '/buy', query: {name: zx.name, zname: zx.zname, method: 'zhuanchang'}})
        window.open(routeUrl.href, '_blank')
      }
    },
    link (name) {
      let routeUrl = this.$router.resolve({path: 'buy', query: {name: name, method: 'putong'}})
      window.open(routeUrl.href, '_blank')
    },
    zlink (name) {
      let routeUrl = this.$router.resolve({path: '/special', query: {name: name}})
      window.open(routeUrl.href, '_blank')
    }
  },
  mounted: function () {
    this.getjpzc()
    this.getjj()
    this.getzx()
    this.getgczx()
    this.getzghh()
    this.gethj()
    this.geturls()
  }
}
</script>
<style scoped>
.link:visited .link:hover .link:link .link:active {
  color: black;
}
</style>
