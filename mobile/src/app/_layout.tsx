import { MaterialCommunityIcons } from '@expo/vector-icons';
import { Stack, useRouter } from 'expo-router';
import { useEffect, useState } from 'react';
import { Platform, StatusBar, StyleSheet, Text, TouchableOpacity, View } from 'react-native';

export default function RootLayout() {
  const [isSidebarOpen, setIsSidebarOpen] = useState(false);
  const [currentTime, setCurrentTime] = useState('00h:00m:00s');
  const router = useRouter();

  const toggleSidebar = () => setIsSidebarOpen(!isSidebarOpen);

  useEffect(() => {
    const updateTime = () => {
      const now = new Date();
      const hrs = String(now.getHours()).padStart(2, '0');
      const mins = String(now.getMinutes()).padStart(2, '0');
      const secs = String(now.getSeconds()).padStart(2, '0');
      setCurrentTime(`${hrs}h:${mins}m:${secs}s`);
    };

    updateTime();
    const timeInterval = setInterval(updateTime, 1000);
    return () => clearInterval(timeInterval);
  }, []);

  const sidebarIcons = [
    { name: 'sprout', label: 'Grow' },
    { name: 'apps', label: 'Plots' }, 
    { name: 'target', label: 'Targets' },
    { name: 'leaf', label: 'Nutrients' },
    { name: 'chart-timeline-variant', label: 'Analytics' },
    { name: 'play-circle-outline', label: 'Automation' },
    { name: 'alert-outline', label: 'Alerts' },
    { name: 'cog-outline', label: 'Settings' },
  ];

  return (
    <View style={styles.appContainer}>
      <StatusBar hidden={true} />

      {/* CUSTOM TOP APPBAR WITH HORIZONTAL DIVIDER */}
      <View style={styles.customAppBar}>
        {/* Left Side: Stacking Title and Icon */}
        <TouchableOpacity onPress={toggleSidebar} activeOpacity={0.7} style={styles.headerLeftStack}>
          <Text style={styles.brandTitleText}>FarmSim</Text>
          <MaterialCommunityIcons
            name="sprout"
            size={24} 
            color="#00E639"
            style={styles.brandIcon}
          />
        </TouchableOpacity>

        {/* Right Side: Clean vertical text strings */}
        <View style={styles.headerRightColumn}>
          <Text style={styles.timeHighlight}>{currentTime}</Text>
          <Text style={styles.metaDataText}>date 28/09/26</Text>
          <Text style={styles.metaDataText}>system online 00d:00h:00m</Text>
        </View>
      </View>

      {/* Main Screen Navigation Stack */}
      <Stack screenOptions={{ headerShown: false }}>
        <Stack.Screen name="index" />
        <Stack.Screen name="plots" /> 
      </Stack>

      {/* Slide-out Sidebar Drawer Overlay */}
      {isSidebarOpen && (
        <View style={styles.sidebarOverlay}>
          <TouchableOpacity style={styles.closeArea} onPress={toggleSidebar} />
          
          <View style={styles.drawerColumn}>
            {sidebarIcons.map((icon, index) => (
              <TouchableOpacity 
                key={index} 
                style={styles.iconWrapper} 
                onPress={() => {
                  toggleSidebar();
                  if (icon.label === 'Plots') {
                    router.push('/plots' as any); 
                  } else if (icon.label === 'Grow') {
                    router.push('/plots' as any); 
                  } else if (icon.label === 'Nutrients') {
                    router.push('/nutrients' as any);
                  } else if (icon.label === 'Targets') { 
                    router.push('/targets' as any); 
                  } else if (icon.label === 'Analytics') {
                  router.push('/analytics' as any); 
                  } else if (icon.label === 'Automation') {
                  router.push('/automation' as any); 
                  } else if (icon.label === 'Alerts') {
                  router.push('/alerts' as any);
                  } else if (icon.label === 'Settings') {
                  router.push('/settings' as any);
                  }
                }}
              >
                <MaterialCommunityIcons
                  name={icon.name as any}
                  size={26}
                  color={icon.label === 'Plots' ? '#00E639' : '#455545'} 
                />
                <Text style={[styles.iconLabel, icon.label === 'Plots' && styles.activeText]}>
                  {icon.label}
                </Text>
              </TouchableOpacity>
            ))}
          </View>
        </View>
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  appContainer: { 
    flex: 1, 
    backgroundColor: '#111411', 
  },
  customAppBar: {
    height: 90, 
    backgroundColor: '#111411',
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingHorizontal: 16,
    paddingTop: 10,
    // ADDED: Thin separating border line matching the sidebar styling
    borderBottomWidth: 1,
    borderBottomColor: '#1A221A', 
  },
  headerLeftStack: {
    alignItems: 'flex-start',
    justifyContent: 'center',
  },
  brandTitleText: {
    color: '#E0E7E0', 
    fontSize: 22, 
    fontWeight: 'bold',
    letterSpacing: 0.5,
  },
  brandIcon: {
    marginTop: 4, 
  },
  headerRightColumn: {
    alignItems: 'flex-end',
    justifyContent: 'center',
    backgroundColor: '#111411',
    borderWidth: 0,
  },
  timeHighlight: {
    color: '#00E639',
    fontSize: 13,
    fontWeight: '700',
    backgroundColor: 'transparent',
    fontFamily: Platform.OS === 'ios' ? 'Courier' : 'monospace',
  },
  metaDataText: {
    color: '#455545',
    fontSize: 10,
    fontWeight: '500',
    lineHeight: 12,
    backgroundColor: 'transparent',
    textTransform: 'lowercase',
  },
  sidebarOverlay: { position: 'absolute', top: 0, bottom: 0, left: 0, right: 0, flexDirection: 'row', backgroundColor: 'rgba(17, 20, 17, 0.4)', zIndex: 9999 },
  closeArea: { position: 'absolute', top: 0, bottom: 0, left: 120, right: 0 },
  drawerColumn: { width: 120, height: '100%', backgroundColor: '#111411', borderRightWidth: 1, borderRightColor: '#1A221A', alignItems: 'center', paddingTop: 60, gap: 25 },
  iconWrapper: { alignItems: 'center', justifyContent: 'center', width: '100%', paddingVertical: 8 },
  iconLabel: { fontSize: 10, color: '#455545', marginTop: 4, fontWeight: '500' },
  activeText: { color: '#00E639' },
});
