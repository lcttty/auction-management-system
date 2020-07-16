<template>
  <el-menu @select="select" :default-active="$route.path" class="el-menu-demo" mode="horizontal" background-color="#545c64" text-color="#fff" active-text-color="#ffd04b">
    <el-menu-item index="/index">竞拍首页</el-menu-item>
    <el-menu-item index="/special">拍卖专场</el-menu-item>
    <el-submenu index="/paimai/all">
      <template slot="title">拍卖分类</template>
      <el-menu-item index="/paimai/zghh">中国绘画</el-menu-item>
      <el-menu-item index="/paimai/sfzk">书法篆刻</el-menu-item>
      <el-menu-item index="/paimai/xhds">西画雕塑</el-menu-item>
      <el-menu-item index="/paimai/gczx">古瓷杂项</el-menu-item>
      <el-menu-item index="/paimai/ddgy">当代工艺</el-menu-item>
      <el-menu-item index="/paimai/jp">酒品</el-menu-item>
    </el-submenu>
    <el-menu-item index="/news">新闻公告</el-menu-item>
    <div v-if="login" style="float:right">
      <el-link href="/#/login" :underline="false"><i class="el-icon-user-solid" style="color: #fff;font-size:15px;padding:20px">登录</i></el-link>
      <el-link href="/#/register" :underline="false"><i class="el-icon-user" style="color:#fff;font-size:15px;padding:20px">注册</i></el-link>
    </div>
    <div v-else>
      <el-dropdown style="float:right" @command="handleCommand">
      <span class="el-dropdown-link">
        <i class="el-icon-s-custom" style="color: #fff;font-size:20px;padding:20px"></i>
      </span>
      <el-dropdown-menu slot="dropdown">
        <el-dropdown-item command="a">个人中心</el-dropdown-item>
        <el-dropdown-item command="b">退出</el-dropdown-item>
      </el-dropdown-menu>
      </el-dropdown>
    </div>
  </el-menu>
</template>
<script>
export default {
  name: 'NavMenu',
  data () {
    return {
      login: true
    }
  },
  methods: {
    select (keypath) {
      this.$router.push({path: keypath})
    },
    panduan () {
      this.$axios({method: 'get', url: 'product/panduan'})
        .then(response => {
          if (response.data === 'true') {
            this.login = false
          } else {
            this.login = true
          }
        })
    },
    handleCommand (command) {
      if (command === 'a') {
        this.activeIndex = '1'
        this.$router.push({path: '/myself'})
      } else if (command === 'b') {
        this.$axios({method: 'post', url: 'product/zhuxiao'})
          .then(response => {
            console.log(response.data === true)
            if (response.data === true) {
              this.$message({message: '注销成功,即将跳转', type: 'success'})
              setTimeout(() => {
                this.$router.push({path: '/login'})
              }, 2000)
            } else {
              this.$message.error('数据库异常')
            }
          })
      }
    }
  },
  mounted: function () {
    this.panduan()
  }
}
</script>
